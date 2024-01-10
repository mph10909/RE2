public interface ISavable<T>
{
    T GetSavableData();

    void SetFromSaveData(T savedData);
}