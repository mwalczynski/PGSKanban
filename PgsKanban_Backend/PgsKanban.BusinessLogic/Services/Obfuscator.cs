using Cryptography.Obfuscation;
using PgsKanban.BusinessLogic.Interfaces;

namespace PgsKanban.BusinessLogic.Services
{
    public class Obfuscation : IObfuscator
    {
        private readonly Obfuscator _obfuscator;

        public Obfuscation()
        {
            _obfuscator = new Obfuscator();
        }

        public string Obfuscate(int value)
        {
            var result = _obfuscator.Obfuscate(value);
            return result;
        }

        public int Deobfuscate(string value)
        {
            var result = _obfuscator.Deobfuscate(value);
            return result;
        }
    }
}
