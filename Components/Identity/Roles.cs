namespace Identity;

[Flags]
public enum Roles
{
    NONE = 0,
    AUTH = 1,
    USER = AUTH << 1,
    ADMIN = AUTH << 2,
}
