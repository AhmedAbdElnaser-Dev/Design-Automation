using System.Text.Json.Serialization;

namespace DesignAutomation.API.Services.Aps;

public class ApsTokenResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = default!;
    [JsonPropertyName("token_type")] public string TokenType { get; set; } = "Bearer";
    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
}

public class CreateBucketRequest
{
    public string BucketKey { get; set; } = default!;
    public string PolicyKey { get; set; } = "transient";
}

public class BucketDetails
{
    [JsonPropertyName("bucketKey")] public string BucketKey { get; set; } = default!;
    [JsonPropertyName("bucketOwner")] public string? BucketOwner { get; set; }
    [JsonPropertyName("createdDate")] public long CreatedDate { get; set; }
    [JsonPropertyName("policyKey")] public string? PolicyKey { get; set; }
    [JsonPropertyName("permissions")] public List<BucketPermission>? Permissions { get; set; }
}

public class BucketPermission
{
    [JsonPropertyName("authId")] public string? AuthId { get; set; }
    [JsonPropertyName("access")] public string? Access { get; set; }
}

public class BucketListResponse
{
    [JsonPropertyName("items")] public List<BucketDetails> Items { get; set; } = new();
    [JsonPropertyName("next")] public string? Next { get; set; }
}

public class ObjectDetails
{
    [JsonPropertyName("bucketKey")] public string BucketKey { get; set; } = default!;
    [JsonPropertyName("objectKey")] public string ObjectKey { get; set; } = default!;
    [JsonPropertyName("objectId")] public string ObjectId { get; set; } = default!;
    [JsonPropertyName("size")] public long Size { get; set; }
    [JsonPropertyName("contentType")] public string? ContentType { get; set; }
    [JsonPropertyName("location")] public string? Location { get; set; }
}

public class ObjectListResponse
{
    [JsonPropertyName("items")] public List<ObjectDetails> Items { get; set; } = new();
    [JsonPropertyName("next")] public string? Next { get; set; }
}

public class SignedS3UploadResponse
{
    [JsonPropertyName("uploadKey")] public string UploadKey { get; set; } = default!;
    [JsonPropertyName("uploadExpiration")] public string? UploadExpiration { get; set; }
    [JsonPropertyName("urlExpiration")] public string? UrlExpiration { get; set; }
    [JsonPropertyName("urls")] public List<string> Urls { get; set; } = new();
}

public class SignedS3UploadCompleteRequest
{
    [JsonPropertyName("uploadKey")] public string UploadKey { get; set; } = default!;
    [JsonPropertyName("size")] public long? Size { get; set; }
    [JsonPropertyName("contentType")] public string? ContentType { get; set; }
}

public class SignedS3DownloadResponse
{
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonPropertyName("url")] public string? Url { get; set; }
    [JsonPropertyName("size")] public long Size { get; set; }
    [JsonPropertyName("sha1")] public string? Sha1 { get; set; }
    [JsonPropertyName("contentType")] public string? ContentType { get; set; }
    [JsonPropertyName("expiration")] public long Expiration { get; set; }
}
