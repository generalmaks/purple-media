namespace PurpleMediaRest.Services.Dto.Tweet;

public record TweetDto(
    int Id,
    int AuthorId,
    string Content,
    int? ParentTweetId,
    DateTime CreatedAt);