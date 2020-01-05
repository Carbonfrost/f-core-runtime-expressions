#
# Investigate `make help` to see a list of targets and their descriptions.
#
.PHONY: dotnet/generate

## Generate generated code
dotnet/generate:
	srgen -c Carbonfrost.Commons.Core.Runtime.Expressions.Resources.SR \
		-r Carbonfrost.Commons.Core.Runtime.Expressions.Automation.SR \
		--resx \
		dotnet/src/Carbonfrost.Commons.Core.Runtime.Expressions/Automation/SR.properties

## Execute dotnet unit tests
dotnet/test: dotnet/publish -dotnet/test

-dotnet/test:
	fspec -i dotnet/test/Carbonfrost.UnitTests.Core.Runtime.Expressions/Content \
		dotnet/test/Carbonfrost.UnitTests.Core.Runtime.Expressions/bin/$(CONFIGURATION)/netcoreapp3.0/publish/Carbonfrost.Commons.Core.dll \
		dotnet/test/Carbonfrost.UnitTests.Core.Runtime.Expressions/bin/$(CONFIGURATION)/netcoreapp3.0/publish/Carbonfrost.Commons.Core.Runtime.Expressions.dll \
		dotnet/test/Carbonfrost.UnitTests.Core.Runtime.Expressions/bin/$(CONFIGURATION)/netcoreapp3.0/publish/Carbonfrost.UnitTests.Core.Runtime.Expressions.dll

include eng/.mk/*.mk
