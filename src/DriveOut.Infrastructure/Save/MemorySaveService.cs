using DriveOut.Core.Save;

namespace DriveOut.Infrastructure.Save;

public sealed class MemorySaveService : ISaveService
{
    private SaveData? _stored;

    public bool Exists => _stored is not null;

    public void Save(SaveData data)
    {
        ArgumentNullException.ThrowIfNull(data);
        _stored = data;
    }

    public SaveData? Load() => _stored;

    public void Delete() => _stored = null;
}
