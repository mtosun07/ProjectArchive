using System;
using System.Collections.Generic;
using System.Linq;

namespace TSN.Universe.DesktopApp
{
    internal class MatterTraitListForm<TMatter, TItem> : ListFormBase
        where TMatter : Matter
    {
        private MatterTraitListForm(string title, TMatter matter, Func<TMatter, ICollection<TItem>> itemSelector, Func<TItem, string> itemToString, Action<TItem> itemOnClick)
            : base(title, matter?.ToString() ?? throw new ArgumentNullException(nameof(matter)), $"[{matter.GetType().Name} | {matter.Id}]")
        {
            if (itemSelector == null)
                throw new ArgumentNullException(nameof(itemSelector));
            _matter = matter;
            _itemSelector = itemSelector;
            _itemToString = itemToString ?? new Func<TItem, string>(x => x.ToString());
            _itemOnClick = itemOnClick;
        }


        private readonly Func<TMatter, ICollection<TItem>> _itemSelector;
        private readonly Func<TItem, string> _itemToString;
        private readonly Action<TItem> _itemOnClick;
        private readonly TMatter _matter;
        private TItem[] _items;



        public static void ShowInstantce(string title, TMatter matter, Func<TMatter, ICollection<TItem>> itemSelector, Func<TItem, string> itemToString = null, Action<TItem> itemOnClick = null)
        {
            var frm = new MatterTraitListForm<TMatter, TItem>(title, matter, itemSelector, itemToString, itemOnClick);
            AddInstance(frm);
            frm.Show();
        }

        protected override string[] GetItems(out string subInfo)
        {
            _items = _itemSelector(_matter)?.ToArray() ?? new TItem[0];
            subInfo = $"Generation {_matter.OwnerUniverse.CurrentGeneration.Value}, Count: {_items.Length}";
            return _items.Select(x => _itemToString(x)).ToArray();
        }
        protected override void OnItemClicked(int index) => _itemOnClick(_items[index]);
    }
}