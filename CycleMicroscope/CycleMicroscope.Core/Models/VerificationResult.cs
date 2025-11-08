using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleMicroscope.Core.Models
{
    /// <summary>
    /// Результат проверки верификационных условий корректности цикла
    /// </summary>
    public class VerificationResult
    {
        /// <summary>
        /// Название проверяемого условия
        /// </summary>
        public string ConditionName { get; set; }

        /// <summary>
        /// Формула условия в математической нотации
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// Описание условия на русском языке
        /// </summary>
        public string RussianDescription { get; set; }

        /// <summary>
        /// Результат проверки условия
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Детальное сообщение о результате проверки
        /// </summary>
        public string Message { get; set; }
    }
}
