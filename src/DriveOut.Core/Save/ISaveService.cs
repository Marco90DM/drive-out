namespace DriveOut.Core.Save;

public interface ISaveService
{
    void Save(SaveData data);
    SaveData? Load();
    void Delete();
    bool Exists { get; }
}
