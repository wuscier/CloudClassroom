using ZOOM_SDK_DOTNET_WRAP;

namespace CloudClassroom.Helpers
{
    public class Translator
    {
        public static string TranslateSDKError(SDKError error)
        {
            string msg = string.Empty;

            switch (error)
            {
                case SDKError.SDKERR_SUCCESS:
                    msg = "成功！";
                    break;
                case SDKError.SDKERR_NO_IMPL:
                    msg = "该功能暂未实现！";
                    break;
                case SDKError.SDKERR_WRONG_USEAGE:
                    msg = "错误的用法！";
                    break;
                case SDKError.SDKERR_INVALID_PARAMETER:
                    msg = "无效的参数！";
                    break;
                case SDKError.SDKERR_MODULE_LOAD_FAILED:
                    msg = "模块加载失败！";
                    break;
                case SDKError.SDKERR_MEMORY_FAILED:
                    msg = "内存错误！";
                    break;
                case SDKError.SDKERR_SERVICE_FAILED:
                    msg = "服务错误！";
                    break;
                case SDKError.SDKERR_UNINITIALIZE:
                    msg = "未初始化！";
                    break;
                case SDKError.SDKERR_UNAUTHENTICATION:
                    msg = "没有认证！";
                    break;
                case SDKError.SDKERR_NORECORDINGINPROCESS:
                    msg = "没有正在进行中的录制！";
                    break;
                case SDKError.SDKERR_TRANSCODER_NOFOUND:
                    msg = "未找到转码器！";
                    break;
                case SDKError.SDKERR_VIDEO_NOTREADY:
                    msg = "视频未就绪！";
                    break;
                case SDKError.SDKERR_NO_PERMISSION:
                    msg = "没有权限！";
                    break;
                case SDKError.SDKERR_UNKNOWN:
                    msg = "未知错误！";
                    break;
                case SDKError.SDKERR_OTHER_SDK_INSTANCE_RUNNING:
                    msg = "另一个服务在运行中！";
                    break;
                default:
                    msg = "未知消息！";
                    break;
            }

            return msg;
        }
    }
}
