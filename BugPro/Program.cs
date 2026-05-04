using System;
using Stateless;

namespace BugPro
{
    public class Bug
    {
        public enum State
        {
            New,
            Triaged,
            NeedInfo,
            Duplicate,
            NotABug,
            InProgress,
            Fixed,
            Closed,
            Reopened
        }

        public enum Trigger
        {
            Triage,
            RequestInfo,
            ProvideInfo,
            MarkDuplicate,
            Reject,
            StartFix,
            Resolve,
            Close,
            Reopen
        }

        private readonly StateMachine<State, Trigger> _machine;

        public State CurrentState => _machine.State;

        public Bug()
        {
            _machine = new StateMachine<State, Trigger>(State.New);

            _machine.Configure(State.New)
                .Permit(Trigger.Triage, State.Triaged);

            _machine.Configure(State.Triaged)
                .Permit(Trigger.RequestInfo, State.NeedInfo)
                .Permit(Trigger.MarkDuplicate, State.Duplicate)
                .Permit(Trigger.Reject, State.NotABug)
                .Permit(Trigger.StartFix, State.InProgress);

            _machine.Configure(State.NeedInfo)
                .Permit(Trigger.ProvideInfo, State.Triaged);

            _machine.Configure(State.InProgress)
                .Permit(Trigger.Resolve, State.Fixed);

            _machine.Configure(State.Fixed)
                .Permit(Trigger.Close, State.Closed);

            _machine.Configure(State.Closed)
                .Permit(Trigger.Reopen, State.Reopened);

            _machine.Configure(State.Reopened)
                .Permit(Trigger.StartFix, State.InProgress);
        }

        public void Fire(Trigger trigger)
        {
            _machine.Fire(trigger);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var bug = new Bug();

            Console.WriteLine($"State: {bug.CurrentState}");

            bug.Fire(Bug.Trigger.Triage);
            Console.WriteLine($"State: {bug.CurrentState}");

            bug.Fire(Bug.Trigger.StartFix);
            Console.WriteLine($"State: {bug.CurrentState}");

            bug.Fire(Bug.Trigger.Resolve);
            Console.WriteLine($"State: {bug.CurrentState}");

            bug.Fire(Bug.Trigger.Close);
            Console.WriteLine($"State: {bug.CurrentState}");
        }
    }
}
