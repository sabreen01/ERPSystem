namespace ERPSystem.Application.Helper.models;

public record RequestResult<T>(
    T? Data,
    bool IsSuccess,
    string Message
);
