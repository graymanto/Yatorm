CREATE TABLE dbo.SingleStringTestTable ([Id] [UniqueIdentifier] NOT NULL  DEFAULT newid() PRIMARY KEY, [TestString] [nvarchar] (100) NOT NULL)
GO

CREATE TABLE [dbo].[TypeTestTable](
	[Id] [uniqueidentifier] NOT NULL,
	[TestString] [nchar](100) NULL,
	[TestNullInt] [int] NULL,
	[TestNullBigInt] [bigint] NULL,
	[TestInt] [int] NOT NULL,
	[TestBigInt] [int] NOT NULL,
	[TestNullDate] [datetime] NULL,
	[TestDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TypeTestTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
