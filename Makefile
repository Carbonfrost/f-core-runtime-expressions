#
# Investigate `make help` to see a list of targets and their descriptions.
#
.PHONY: dotnet/generate

-include eng/.mk/*.mk

## Generate generated code
dotnet/generate:
	$(Q) srgen -c Carbonfrost.Commons.Core.Runtime.Expressions.Resources.SR \
		-r Carbonfrost.Commons.Core.Runtime.Expressions.Automation.SR \
		--resx \
		dotnet/src/Carbonfrost.Commons.Core.Runtime.Expressions/Automation/SR.properties
	$(Q) dotnet t4 dotnet/src/Carbonfrost.Commons.Core.Runtime.Expressions/Automation/TextTemplates/Expressions.tt

## Execute dotnet unit tests
dotnet/test: dotnet/publish -dotnet/test

-dotnet/test:
	$(Q) fspec -i dotnet/test/Carbonfrost.UnitTests.Core.Runtime.Expressions/Content \
		dotnet/test/Carbonfrost.UnitTests.Core.Runtime.Expressions/bin/$(CONFIGURATION)/netcoreapp3.0/publish/Carbonfrost.UnitTests.Core.Runtime.Expressions.dll

## Run unit tests with code coverage
dotnet/cover: dotnet/publish -check-command-coverlet
	$(Q) coverlet \
		--target "make" \
		--targetargs "-- -dotnet/test" \
		--format lcov \
		--output lcov.info \
		--exclude-by-attribute 'Obsolete' \
		--exclude-by-attribute 'GeneratedCode' \
		--exclude-by-attribute 'CompilerGenerated' \
		dotnet/test/Carbonfrost.UnitTests.Core.Runtime.Expressions/bin/$(CONFIGURATION)/netcoreapp3.0/publish/Carbonfrost.UnitTests.Core.Runtime.Expressions.dll
