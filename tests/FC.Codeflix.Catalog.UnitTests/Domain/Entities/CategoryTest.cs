using FC.Codeflix.Catalog.Domain;
using FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;


    public CategoryTest(CategoryTestFixture categoryTestFixture)
    {
        _categoryTestFixture = categoryTestFixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category")]
    public void Instantiate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var dateTimeBefore = DateTime.Now;

        var category = new Category(validCategory.Name, validCategory.Description);

        var dateTimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.Equal(validCategory.Name, category.Name);
        Assert.Equal(validCategory.Description, category.Description);
        Assert.True(category.IsActive);
        Assert.True(category.CreatedAt >= dateTimeBefore);
        Assert.True(category.CreatedAt <= dateTimeAfter);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description, isActive);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new Category(name!, validCategory.Description);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category")]
    [InlineData("ab")]
    [InlineData("a")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new Category(name!, validCategory.Description);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at least 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsBiggerThan255Characters))]
    [Trait("Domain", "Category")]
    public void InstantiateErrorWhenNameIsBiggerThan255Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        
        Action action =
            () => new Category(invalidName, validCategory.Description);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsBiggerThan10000Characters))]
    [Trait("Domain", "Category")]
    public void InstantiateErrorWhenDescriptionIsBiggerThan10000Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidDescription = String.Join(null, Enumerable.Range(0, 10001).Select(_ => "a").ToArray());
        
        Action action =
            () => new Category(validCategory.Name, invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10000 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new Category(validCategory.Name, null!);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(ShouldActivateCategory))]
    [Trait("Domain", "Category")]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldActivateCategory(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description, isActive);
        category.Activate();
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(ShouldDeactivateCategory))]
    [Trait("Domain", "Category")]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldDeactivateCategory(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description, isActive);
        category.Deactivate();
        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(ShouldUpdateCategoryWhenTheParametersAreValid))]
    [Trait("Domain", "Category")]
    public void ShouldUpdateCategoryWhenTheParametersAreValid()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var newValues = new
        {
            Name = "New Category Name",
            Description = "New Category Description",
        };

        category.Update(newValues.Name, newValues.Description);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
    }

    [Fact(DisplayName = nameof(ShouldUpdateOnlyTheName))]
    [Trait("Domain", "Category")]
    public void ShouldUpdateOnlyTheName()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var currentDescription = category.Description;
        var newName = "New Category Name";

        category.Update(newName);

        Assert.Equal(newName, category.Name);
        Assert.Equal(currentDescription, category.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = _categoryTestFixture.GetValidCategory();
        
        Action action =
            () => category.Update(name!);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category")]
    [InlineData("ab")]
    [InlineData("a")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string? name)
    {
        var category = _categoryTestFixture.GetValidCategory();

        Action action =
            () => category.Update(name!);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at least 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsBiggerThan255Characters))]
    [Trait("Domain", "Category")]
    public void UpdateErrorWhenNameIsBiggerThan255Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();

        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        
        Action action =
            () => category.Update(invalidName, category.Description);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsBiggerThan10000Characters))]
    [Trait("Domain", "Category")]
    public void UpdateErrorWhenDescriptionIsBiggerThan10000Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();

        var invalidDescription = String.Join(null, Enumerable.Range(0, 10001).Select(_ => "a").ToArray());
        
        Action action =
            () => category.Update(category.Name, invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10000 characters long", exception.Message);
    }



}
