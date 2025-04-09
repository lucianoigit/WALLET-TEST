using INFRAESTRUCTURE.Helpers;
using Xunit;

namespace TEST.Helpers;

public class PagedListTests
{
    [Fact]
    public void CreateFromList_ReturnsCorrectPaginationMetadata()
    {
        // Arrange
        var data = Enumerable.Range(1, 50).ToList();
        int page = 2;
        int pageSize = 10;

        // Act
        var pagedList = PagedList<int>.CreateFromList(data, pageSize, page, data.Count);

        // Assert
        Assert.Equal(10, pagedList.Items.Count);
        Assert.Equal(2, pagedList.Page);
        Assert.Equal(10, pagedList.PageSize);
        Assert.Equal(50, pagedList.TotalCount);
        Assert.Equal(5, pagedList.TotalPages);
        Assert.True(pagedList.HasNextPage);
        Assert.True(pagedList.HasPreviousPage);
        Assert.Equal(11, pagedList.Items.First());
    }
}

