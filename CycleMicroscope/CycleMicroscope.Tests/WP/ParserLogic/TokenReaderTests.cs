using CycleMicroscope.WP.ParserLogic;
using Xunit;

namespace CycleMicroscope.Tests.WP.ParserLogic
{
    public class TokenReaderTests
    {
        [Fact]
        public void Read_WithTokens_ReturnsTokensInOrder()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "2")
            };
            var reader = new TokenReader(tokens);

            // Act & Assert
            Assert.Equal("1", reader.Read().Value);
            Assert.Equal("+", reader.Read().Value);
            Assert.Equal("2", reader.Read().Value);
            Assert.True(reader.IsEnd);
        }

        [Fact]
        public void Peek_DoesNotAdvancePosition()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new Token(TokenType.Identifier, "x"),
                new Token(TokenType.Operator, ":=")
            };
            var reader = new TokenReader(tokens);

            // Act
            var firstPeek = reader.Peek();
            var secondPeek = reader.Peek();

            // Assert
            Assert.Same(firstPeek, secondPeek);
            Assert.Equal("x", firstPeek.Value);
            Assert.False(reader.IsEnd);
        }

        [Fact]
        public void RemainingTokens_ReturnsCorrectCount()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Operator, "-"), // Унарный или бинарный минус
                new Token(TokenType.Number, "2")
            };
            var reader = new TokenReader(tokens);

            // Act & Assert
            Assert.Equal(3, reader.RemainingTokens);
            reader.Read();
            Assert.Equal(2, reader.RemainingTokens);
            reader.Read();
            Assert.Equal(1, reader.RemainingTokens);
        }
    }
}