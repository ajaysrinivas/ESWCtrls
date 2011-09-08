using System;
using System.Web.UI;

namespace ESWCtrls.Internal
{
    internal sealed class LimitCollection : ControlCollection
    {
        private int _limit;

        public LimitCollection(Control owner, int limit)
            : base(owner)
        {
            _limit = limit;
        }

        internal void AddInternal(Control child)
        {
            if (Count < _limit)
                base.Add(child);
            else
                throw new InvalidOperationException("This collection only accepts one child");
        }

        internal void ClearInternal()
        {
            base.Clear();
        }

        public override void Add(System.Web.UI.Control child)
        {
            throw new InvalidOperationException("You cannot modify control collection");
        }

        public override void AddAt(int index, System.Web.UI.Control child)
        {
            throw new InvalidOperationException("You cannot modify control collection");
        }

        public override void Clear()
        {
            throw new InvalidOperationException("You cannot modify control collection");
        }
    }
}
