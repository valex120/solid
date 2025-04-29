using SmartConvertor.Models;

namespace SmartConvertor.Interfaces
{
    public interface IStampService
    {
        byte[] AddStamp(byte[] pdf, StampInfo stampInfo);
    }
}
