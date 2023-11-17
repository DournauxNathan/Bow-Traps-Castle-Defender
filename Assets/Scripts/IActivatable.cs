public interface IActivatable
{
    bool IsBroken { get; }
    bool Repairable { get; }
    float RepairTime { get; }

    void Break();
    void Repair();

}