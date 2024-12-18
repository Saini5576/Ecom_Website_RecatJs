using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.BaseResponse;
using System.Text.Json.Serialization;

using System.Text.Json.Serialization;

// Base Error Model
public class ErrorModel
{
    public string? Error { get; set; }
    public string? ErrorLocation { get; set; }
    public string? UserMessage { get; set; }
    public string? DeveloperMessage { get; set; }
}

// Base Response Class (non-generic)
public class Response
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; init; }

    public bool Success { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ErrorModel? ErrorDetails { get; init; }

    // Constructor
    protected Response(string? message = null, bool success = false, ErrorModel? errorDetails = null)
    {
        Message = message;
        Success = success;
        ErrorDetails = errorDetails;
    }

    // Factory Methods
    public static Response SuccessResponse(string? message = null)
    {
        return new Response(message, success: true);
    }

    public static Response FailureResponse(string? message = null, ErrorModel? errorDetails = null)
    {
        return new Response(message, success: false, errorDetails);
    }
}

// Generic Response Class
public class Response<TEntity> : Response
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TEntity? Content { get; private set; }

    // Constructor
    private Response(TEntity? content = default, string? message = null, bool success = false, ErrorModel? errorDetails = null)
        : base(message, success, errorDetails)
    {
        Content = content;
    }

    // Factory Methods
    public static Response<TEntity> SuccessResponse(TEntity? content = default, string? message = null)
    {
        return new Response<TEntity>(content, message, success: true);
    }

    public static Response<TEntity> FailureResponse(string? message = null, ErrorModel? errorDetails = null)
    {
        return new Response<TEntity>(content: default, message, success: false, errorDetails);
    }
}


