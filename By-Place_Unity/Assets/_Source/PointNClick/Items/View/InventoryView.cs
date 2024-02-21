using System.Collections.Generic;
using PointNClick.Cursor.Manager;
using PointNClick.Cursor.Sensitive;
using PointNClick.Cursor.Sensitive.Data;
using PointNClick.Items.Data;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace PointNClick.Items.View
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private UIDocument canvas;
        [SerializeField] private VisualTreeAsset itemSlotTree;
        [SerializeField] private CursorConfigSO toggleCursorConfigSO;
        [SerializeField] private Sprite closedInventorySprite;
        [SerializeField] private Sprite openedInventorySprite;

        private ICursorManager _cursorManager;
        private UICursorSensitive _toggleCursorSensitive;
        
        private VisualElement _root;

        private VisualElement _toggleBtn;
        private VisualElement _itemSlotsHolder;
        
        private Dictionary<int, ItemView> _itemsViews = new();
        private List<ItemView> _freeItemViews = new();

        private bool _showItems;

        [Inject]
        private void Inject(ICursorManager cursorManager) => _cursorManager = cursorManager;

        private void Awake()
        {
            _root = canvas.rootVisualElement.Q<VisualElement>("InventoryPanel");

            _toggleBtn = _root.Q<VisualElement>("ToggleBtn");
            _itemSlotsHolder = _root.Q<VisualElement>("ItemSlotsHolder");

            _toggleCursorSensitive = new UICursorSensitive(toggleCursorConfigSO, _cursorManager, _toggleBtn);
            _toggleBtn.style.backgroundImage = Background.FromSprite(closedInventorySprite);
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Bind()
        {
            _toggleBtn.RegisterCallback<MouseDownEvent>(ToggleItems);
            _toggleCursorSensitive.Bind();
        }

        private void Expose()
        {
            _toggleBtn.UnregisterCallback<MouseDownEvent>(ToggleItems);
            _toggleCursorSensitive.Expose();
        }

        private void ToggleItems(MouseDownEvent evt)
        {
            if (!_showItems)
                ShowItems();
            else
                HideItems();
        }

        private void ShowItems()
        {
            _toggleBtn.style.backgroundImage = Background.FromSprite(openedInventorySprite);
            foreach (var kvp in _itemsViews)
                kvp.Value.Show();
            
            _showItems = true;
        }

        private void HideItems()
        {
            _toggleBtn.style.backgroundImage = Background.FromSprite(closedInventorySprite);
            foreach (var kvp in _itemsViews)
                kvp.Value.Hide();
            
            _showItems = false;
        }

        public void AddItem(ItemSO itemSO)
        {
            ItemView itemView;
            if (_freeItemViews.Count == 0)
            {
                var itemRoot = itemSlotTree.CloneTree().Q<VisualElement>("Item");
                itemView = new ItemView(itemRoot, itemSO);
                _itemSlotsHolder.Add(itemView.Root);
            }
            else
            {
                itemView = _freeItemViews[^1];
                _freeItemViews.RemoveAt(_freeItemViews.Count - 1);
            }
            
            _itemsViews.Add(itemSO.Id, itemView);
            if (_showItems)
                itemView.Show();
            else
                itemView.Hide();
        }

        public void RemoveItem(ItemSO itemSO)
        {
            if (!_itemsViews.ContainsKey(itemSO.Id))
                return;

            var itemView = _itemsViews[itemSO.Id];
            _freeItemViews.Add(itemView);
            _itemsViews.Remove(itemSO.Id);
            itemView.Hide();
        }
    }
}