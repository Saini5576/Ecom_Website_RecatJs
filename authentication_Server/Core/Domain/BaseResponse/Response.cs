using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.BaseResponse;
public class Response<TEntity> : ErrorField
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TEntity? Content { get; private set; }

    public Response(TEntity? content = default(TEntity), string? message = default, bool success = default) =>
                         (Message, Content, Success) = (message, content, success);

    public static Response<TEntity?> SuccessResponse() =>
                                     new Response<TEntity?>(success: true);
    public static Response<TEntity?> SuccessResponse(
                    TEntity? content = default(TEntity),
                     string? Message = default) =>
                       new Response<TEntity?>(content, Message, success: true);

    public static Response<TEntity?> FailureResponse(
                 string? Message = default) =>
                   new Response<TEntity?>(message: Message, success: false);

}

public class Response : ErrorField
{
    public Response(string? message = default, bool success = default) =>
                        (Message, Success) = (message, success);

    public static Response SuccessResponse(string? Message = default) =>
                           new Response(message: Message, success: true);

    public static Response FailureResponse(string? Message = default) =>
                          new Response(message: Message, success: false);
}

public class ErrorField 
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; init; }
    public bool Success { get; init; }
}
