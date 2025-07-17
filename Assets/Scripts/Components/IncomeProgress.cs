/// <summary>
///  Счётчик до начисления дохода.
///  elapsed – сколько секунд уже прошло,
///  delay   – полный «круг» до выплаты.
/// </summary>
public struct IncomeProgress {
    public float elapsed;
    public float delay;
}