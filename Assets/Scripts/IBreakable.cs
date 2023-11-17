public interface IBreakable
{
    bool IsBroken { get; }
    bool Repairable { get; set; }
    float RepairTime { get; }

    void Break();
    void Repair(float amount);

}