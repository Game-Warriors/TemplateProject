using GameWarriors.UIDomain.Abstraction;

namespace Common.Extensions
{
    public static class UIExtension
    {
        public static bool IsFreezeCameraMovement(this IScreen screenHandler)
        {
            return (screenHandler.LastScreen?.HasBlackScreen ?? false) || screenHandler.OpenScreenCount > 1;
        }
    }
}
