namespace PgsKanban.BusinessLogic.Interfaces
{
    public interface IObfuscator
    {
        string Obfuscate(int value);
        int Deobfuscate(string value);
    }
}
