namespace SNS.Domain;

public readonly record struct UserId(long Value)
{
    public override readonly string ToString()
    {
        return Value.ToString();
    }
}
