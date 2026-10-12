using FC.Codeflix.Catalog.Domain;
using FC.Codeflix.Catalog.Domain.Validation;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var value = "Some value";

        var exception = Record.Exception(() => DomainValidation.NotNull(value, "Value"));

        Assert.Null(exception);
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? value = null;

        Action action = () => DomainValidation.NotNull(value, "FieldName");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("FieldName should not be null", exception.Message);
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        var value = "Some value";

        var exception = Record.Exception(() => DomainValidation.NotNullOrEmpty(value, "Value"));

        Assert.Null(exception);
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        Action action = () => DomainValidation.NotNullOrEmpty(target, "FieldName");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("FieldName should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("abc", 3)]
    [InlineData("abcdef", 3)]
    public void MinLengthOk(string target, int minLength)
    {
        var exception = Record.Exception(() => DomainValidation.MinLength(target, minLength, "FieldName"));

        Assert.Null(exception);
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("ab", 3)]
    [InlineData("", 1)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        Action action = () => DomainValidation.MinLength(target, minLength, "FieldName");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal($"FieldName should be at least {minLength} characters long", exception.Message);
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("abc", 3)]
    [InlineData("a", 3)]
    public void MaxLengthOk(string target, int maxLength)
    {
        var exception = Record.Exception(() => DomainValidation.MaxLength(target, maxLength, "FieldName"));

        Assert.Null(exception);
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("abcd", 3)]
    [InlineData("ab", 1)]
    public void MaxLengthThrowWhenGreater(string target, int maxLength)
    {
        Action action = () => DomainValidation.MaxLength(target, maxLength, "FieldName");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal($"FieldName should be less or equal {maxLength} characters long", exception.Message);
    }
}
