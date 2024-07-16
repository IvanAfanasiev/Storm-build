public interface ITrigger
{
    void Interact();
    string ActionName();

    void ActionChanges();

    bool Check();
}
