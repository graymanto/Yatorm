.PHONY: setup run build format inspect

setup:
	@echo "Installing..."
	dotnet tool restore
	dotnet husky install

build:
	dotnet build

test:
	dotnet test tests/Yatorm.Tests

format:
	dotnet format
	dotnet csharpier .

checkformat:
	dotnet csharpier --check .
	dotnet format --verify-no-changes

inspect:
	dotnet jb inspectcode Yatorm.sln -o=/dev/stdout -f=text

