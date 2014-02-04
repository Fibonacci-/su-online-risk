using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachines
{
    enum MainState { Start, Initialize, Distribute, Collect, Reinforce, Attack, Fortify, Over};
    /*class State
    {

        public MainState mainState
        {
            get;
            set;
        }
        public State()
        {
            mainState = MainState.Start;
        }
        public void Next()
        {
            lock(this)
            {
                switch (mainState)
                {
                    case MainState.Start:
                        mainState = MainState.Initialize;
                        break;
                    case MainState.Initialize:
                        mainState = MainState.Distribute;
                        break;
                    case MainState.Distribute:
                        mainState = MainState.Collect;
                        break;
                    case MainState.Collect:
                        mainState = MainState.Reinforce;
                        break;
                    case MainState.Reinforce:
                        mainState = MainState.Attack;
                        break;
                    case MainState.Attack:
                        mainState = MainState.Fortify;
                        break;
                    case MainState.Fortify:
                        mainState = MainState.Collect;
                        Activate(true);
                        break;
                    case MainState.Over:
                        mainState = MainState.Over;
                        Activate(true);
                        break;
                }
            }
        }
        public void GameOver()
        {
            lock(this)
            {
                mainState = MainState.Over;
                Activate(true);
            }
        }
    }*/
}
