using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using YatORM.Extensions;

namespace YatORM
{
    public static class DBToTypeConverter
    {
        private static readonly Func<Type, Delegate> _paramMapperFunc =
            new Func<Type, Delegate>(MakeParamMapper).Memoize();

        private static readonly ConcurrentDictionary<string, object> _cachedMappers =
            new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Returns the rows in the data reader as a collection of strongly typed objects. 
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="rdr">
        /// The RDR.
        /// </param>
        /// <returns>
        /// </returns>
        public static IQueryable<T> GetMappedSequence<T>(this IDataReader rdr) where T : new()
        {
            var cols = 0.UpTo(rdr.FieldCount - 1).Select(rdr.GetName).ToList();

            var cacheKey = typeof(T).Name + "|" + string.Join("|", cols);
            var map = (Func<IDataReader, T>)_cachedMappers.GetOrAdd(cacheKey, key => MakeReaderToClassMapper<T>(cols));

            var result = new List<T>();
            while (rdr.Read())
            {
                result.Add(map(rdr));
            }

            return result.AsQueryable();
        }

        /// <summary>
        /// Transforms the values in a strongly typed class into a list of sql parameters. 
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<SqlParameter> TransformClassToSqlParameters<T>(T input)
        {
            var mapper = _paramMapperFunc(input.GetType());
            return mapper.DynamicInvoke(input) as IEnumerable<SqlParameter>;
        }

        /// <summary>
        /// Builds a compiled function that copies from fields of a datareader into properties of a class in the fastest
        /// way possible.
        /// </summary>
        /// <typeparam name="TMapped">
        /// The type of the mapped.
        /// </typeparam>
        /// <param name="cols">
        /// The cols.
        /// </param>
        /// <returns>
        /// </returns>
        private static Func<IDataReader, TMapped> MakeReaderToClassMapper<TMapped>(IEnumerable<string> cols)
        {
            var source = Expression.Parameter(typeof(IDataReader), "reader");
            var dest = Expression.Variable(typeof(TMapped), "dest");

            var variables = new List<ParameterExpression> { dest };
            var body = new List<Expression> { Expression.Assign(dest, Expression.New(typeof(TMapped))), dest };

            var nameOrdinalMap = cols.Select((c, i) => new { Index = i, Name = c })
                .ToDictionary(k => k.Name, v => v.Index);

            var getValueMethod = typeof(IDataRecord).GetMethod("GetValue");

            foreach (var property in typeof(TMapped).GetProperties().Where(p => nameOrdinalMap.ContainsKey(p.Name)))
            {
                var setMethod = property.GetSetMethod();
                if (setMethod == null || setMethod.GetParameters().Length != 1)
                {
                    continue;
                }

                var getValueFromReaderExpression = Expression.Call(
                    source,
                    getValueMethod,
                    Expression.Constant(nameOrdinalMap[property.Name]));

                var valueVariable = Expression.Variable(typeof(object), "readerValue");
                variables.Add(valueVariable);

                body.Add(Expression.Assign(valueVariable, getValueFromReaderExpression));

                var setValueExpression = Expression.Call(
                    dest,
                    setMethod,
                    Expression.Convert(valueVariable, property.PropertyType));

                var ifNotNullExpression =
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Constant(DBNull.Value), valueVariable),
                        setValueExpression);

                body.Add(ifNotNullExpression);
            }

            body.Add(dest);

            var expr = Expression.Lambda<Func<IDataReader, TMapped>>(
                Expression.Block(variables, body.ToArray()),
                source);

            return expr.Compile();
        }

        /// <summary>
        /// Uses the expression api to compile a function that transforms a class into SQL parameters.
        /// </summary>
        /// <returns>
        /// </returns>
        private static Delegate MakeParamMapper(Type paramType)
        {
            var paramConstructor = typeof(SqlParameter).GetConstructor(new[] { typeof(string), typeof(object) });

            if (paramConstructor == null)
            {
                return null;
            }

            var source = Expression.Parameter(paramType, "source");
            var dest = Expression.Variable(typeof(List<SqlParameter>), "dest");

            var allProps = paramType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);
            var addMethod = typeof(List<SqlParameter>).GetMethod("Add");

            var body = new List<Expression> { Expression.Assign(dest, Expression.New(typeof(List<SqlParameter>))) };

            var variables = new List<ParameterExpression> { dest };

            foreach (var prop in allProps)
            {
                var propValue = Expression.Property(source, prop);
                var isNullable = Nullable.GetUnderlyingType(prop.PropertyType) != null;

                var constructNewParameterExpression = Expression.New(
                    paramConstructor,
                    Expression.Constant("@" + prop.Name),
                    Expression.Convert(propValue, typeof(object)));

                if (isNullable)
                {
                    var hasValue = prop.PropertyType.GetProperty("HasValue");

                    var condition = Expression.Property(propValue, hasValue);
                    var constructDbNullParameter = Expression.New(
                        paramConstructor,
                        Expression.Constant("@" + prop.Name),
                        Expression.Constant(DBNull.Value));

                    var addNull = Expression.Call(dest, addMethod, constructDbNullParameter);
                    var addValue = constructNewParameterExpression;

                    body.Add(Expression.IfThenElse(condition, addValue, addNull));
                }
                else
                {
                    body.Add(Expression.Call(dest, addMethod, constructNewParameterExpression));
                }
            }

            body.Add(dest);

            var expr = Expression.Lambda(
                Expression.Block(variables, body.ToArray()),
                source);

            return expr.Compile();
        }
    }
}