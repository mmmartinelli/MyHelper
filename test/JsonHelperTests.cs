using System.Text.Json;
using System.Text.Json.Nodes;
using src;
using test.Mocks;

namespace test;

public class JsonHelperTests
{
    [Fact]
    public void Serialize_NullObject_ShouldReturnsEmptyArrayString()
    {
        // Arrange
        object? input = null;

        // Act
        string actual = JsonHelper.Serialize(input);
        
        // Assert
        Assert.Equal(
            new JsonArray().ToJsonString(),
            actual
        );
    }

    [Fact]
    public void Serialize_MockedObject_ShouldReturnsSerializedStringInPascalCase()
    {
        // Arrange
        JsonHelperMock mock = new(12345, "My Name", 18, "+54 33 3023-1994");

        // Act
        string actual = JsonHelper.Serialize(mock);
        
        // Assert
        string expected = """{"Id":12345,"Name":"My Name","Age":18,"PhoneNumber":"+54 33 3023-1994"}""";
        
        Assert.Equal(
            expected,
            actual
        );
    }

    [Fact]
    public void Serialize_WithNamingPolicy_MockedObject_ShouldReturnsSerializedStringInSnakeCaseLower()
    {
        // Arrange
        JsonHelperMock mock = new(12345, "My Name", 18, "+54 33 3023-1994");

        // Act
        string actual = JsonHelper.Serialize(mock, JsonNamingPolicy.SnakeCaseLower);
        
        // Assert
        string expected = """{"id":12345,"name":"My Name","age":18,"phone_number":"+54 33 3023-1994"}""";

        Assert.Equal(
            expected,
            actual
        );
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Deserialize_EmptyString_ShouldReturnsDefault(string input)
    {
        // Arrange
    
        // Act
        object? actual = JsonHelper.Deserialize<object>(input);
        
        // Assert
        Assert.Null(actual);
    }

    [Theory]
    [InlineData("""{"id":12345,"name":"My Name","age":18,"phonenumber":"+54 33 3023-1994"}""")]
    [InlineData("""{"id":12345,"name":"My Name","age":18,"phoneNumber":"+54 33 3023-1994"}""")]
    [InlineData("""{"Id":12345,"Name":"My Name","Age":18,"PhoneNumber":"+54 33 3023-1994"}""")]
    public void Deserialize_CaseSensitiveString_ShouldReturnsDeserializedObject(string input)
    {
        // Arrange

        // Act
        JsonHelperMock? actual = JsonHelper.Deserialize<JsonHelperMock>(input);
        
        // Assert
        JsonHelperMock expected = new(12345, "My Name", 18, "+54 33 3023-1994");

        Assert.Equal(
            expected,
            actual
        );
    }

    [Theory]
    [InlineData("""{"aa":12345,"bbbb":"My Name","ccc":18,"dddddddddddd":"+54 33 3023-1994"}""")]
    public void DeserializeSilent_WrongObjectMatching_ShouldReturnsDefault(string input)
    {
        // Arrange

        // Act
        JsonHelperMock? actual = JsonHelper.DeserializeSilent<JsonHelperMock>(input);
        
        // Assert
        Assert.Equal(
            new(default, default, default, default),
            actual
        );
    }

    [Theory]
    [InlineData("""{"id":12345,"person":{"name":"My Name","age":18,"phonenumber":"+54 33 3023-1994"}}""", "person", """{"name":"My Name","age":18,"phonenumber":"+54 33 3023-1994"}""")]
    public void GetNode_JsonString_ShouldReturnsJsonStringFromInputedNode(string inputString, string inputNodeName, string expected)
    {
        // Arrange

        // Act
        string actual = JsonHelper.GetNode(inputString, inputNodeName);
        
        // Assert
        Assert.Equal(
            expected,
            actual
        );
    }
}