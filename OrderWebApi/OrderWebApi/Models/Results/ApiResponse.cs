public class ApiResponse<T>
{
    public ApiResponse(StatusType status, T data)
    {
        Status = status;
        Data = data;
    }

    public ApiResponse(StatusType status, string resultMessage)
    {
        Status = status;
        ResultMessage = resultMessage;
    }

    public ApiResponse(StatusType status, string resultMessage, T data)
    {
        Status = status;
        ResultMessage = resultMessage;
        Data = data;
    }

    public ApiResponse(StatusType status, string resultMessage, int errorCode)
    {
        Status = status;
        ResultMessage = resultMessage;
        ErrorCode = errorCode;
    }

    public StatusType Status { get; set; }
    public string ResultMessage { get; set; }
    public int ErrorCode { get; set; }
    public T Data { get; set; }


}
public enum StatusType
{
    Success, Failed
}