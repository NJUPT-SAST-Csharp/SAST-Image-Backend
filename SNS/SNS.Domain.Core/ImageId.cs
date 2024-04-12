namespace SNS.Domain;

public readonly record struct ImageId(long Value)
{
    public override readonly string ToString() => Value.ToString();
}
