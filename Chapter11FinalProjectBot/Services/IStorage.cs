using Chapter11FinalProjectBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter11FinalProjectBot.Services
{
    public interface IStorage
    {
        Session GetSession(long chatId);
    }
}
