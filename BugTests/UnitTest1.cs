using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;
using System;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InitialState_IsNew()
        {
            var bug = new Bug();
            Assert.AreEqual(Bug.State.New, bug.CurrentState);
        }

        [TestMethod]
        public void Can_Triage()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            Assert.AreEqual(Bug.State.Triaged, bug.CurrentState);
        }

        [TestMethod]
        public void Can_Request_Info()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.RequestInfo);
            Assert.AreEqual(Bug.State.NeedInfo, bug.CurrentState);
        }

        [TestMethod]
        public void Can_Return_From_NeedInfo()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.RequestInfo);
            bug.Fire(Bug.Trigger.ProvideInfo);
            Assert.AreEqual(Bug.State.Triaged, bug.CurrentState);
        }

        [TestMethod]
        public void Can_Start_Fix()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.StartFix);
            Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
        }

        [TestMethod]
        public void Can_Resolve()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.StartFix);
            bug.Fire(Bug.Trigger.Resolve);
            Assert.AreEqual(Bug.State.Fixed, bug.CurrentState);
        }

        [TestMethod]
        public void Can_Close()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.StartFix);
            bug.Fire(Bug.Trigger.Resolve);
            bug.Fire(Bug.Trigger.Close);
            Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
        }

        [TestMethod]
        public void Can_Reopen()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.StartFix);
            bug.Fire(Bug.Trigger.Resolve);
            bug.Fire(Bug.Trigger.Close);
            bug.Fire(Bug.Trigger.Reopen);
            Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
        }

        [TestMethod]
        public void Reopened_To_InProgress()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.StartFix);
            bug.Fire(Bug.Trigger.Resolve);
            bug.Fire(Bug.Trigger.Close);
            bug.Fire(Bug.Trigger.Reopen);
            bug.Fire(Bug.Trigger.StartFix);

            Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
        }

        // ❗ Тесты на исключения Stateless

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Cannot_Close_From_New()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Close);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Cannot_Resolve_Without_Fix()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.Resolve);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Cannot_Reopen_If_Not_Closed()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Reopen);
        }

        // Дополнительные тесты (добиваем до 20+)

        [TestMethod]
        public void Mark_As_Duplicate()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.MarkDuplicate);
            Assert.AreEqual(Bug.State.Duplicate, bug.CurrentState);
        }

        [TestMethod]
        public void Reject_Bug()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.Reject);
            Assert.AreEqual(Bug.State.NotABug, bug.CurrentState);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Cannot_StartFix_From_New()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.StartFix);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Cannot_Close_Without_Fixed()
        {
            var bug = new Bug();
            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.StartFix);
            bug.Fire(Bug.Trigger.Close);
        }

        [TestMethod]
        public void Full_Workflow()
        {
            var bug = new Bug();

            bug.Fire(Bug.Trigger.Triage);
            bug.Fire(Bug.Trigger.StartFix);
            bug.Fire(Bug.Trigger.Resolve);
            bug.Fire(Bug.Trigger.Close);

            Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
        }
    }
}