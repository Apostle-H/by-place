using System.Collections.Generic;
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

        private ItemInfoView _itemInfoView;
        
        private UICursorSensitive.Factory _cursorSensitiveFactory;
        private UICursorSensitive _toggleCursorSensitive;
        
        private VisualElement _root;

        private VisualElement _toggleBtn;
        private VisualElement _itemSlotsHolder;
        
        private Dictionary<int, ItemView> _itemsViews = new();
        private List<ItemView> _freeItemViews = new();

        private bool _showItems;

        [Inject]
        private void Inject(ItemInfoView itemInfoView, UICursorSensitive.Factory cursorSensitiveFactory)
        {
            _itemInfoView = itemInfoView;
            
            _cursorSensitiveFactory = cursorSensitiveFactory;
        }

        private void Awake()
        {
            _root = canvas.rootVisualElement.Q<VisualElement>("InventoryPanel");

            _toggleBtn = _root.Q<VisualElement>("ToggleBtn");
            _itemSlotsHolder = _root.Q<VisualElement>("ItemSlotsHolder");

            _toggleCursorSensitive = _cursorSensitiveFactory.Build(toggleCursorConfigSO, _toggleBtn);
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

        private void BindItemView(ItemView itemView)
        {
            itemView.Bind();

            itemView.OnHover += ShowItemInfo;
            itemView.OnUnhover += HideItemInfo;
        }

        private void ExposeItemView(ItemView itemView)
        {
            itemView.Expose();

            itemView.OnHover -= ShowItemInfo;
            itemView.OnUnhover -= HideItemInfo;
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
            {
                kvp.Value.Show();
                BindItemView(kvp.Value);
            }

            _showItems = true;
        }

        private void HideItems()
        {
            _toggleBtn.style.backgroundImage = Background.FromSprite(closedInventorySprite);
            foreach (var kvp in _itemsViews)
            {
                kvp.Value.Hide();
                ExposeItemView(kvp.Value);
            }

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
            {
                itemView.Show();
                BindItemView(itemView);
            }
            else
            {
                itemView.Hide();
                ExposeItemView(itemView);
            }
        }

        public void RemoveItem(ItemSO itemSO)
        {
            if (!_itemsViews.ContainsKey(itemSO.Id))
                return;

            var itemView = _itemsViews[itemSO.Id];
            _freeItemViews.Add(itemView);
            _itemsViews.Remove(itemSO.Id);
            
            itemView.Hide();
            ExposeItemView(itemView);
        }

        public void ShowItemInfo(ItemView itemView)
        {
            _itemInfoView.Show();
            _itemInfoView.UpdateInfo(itemView.TargetItem.Name, itemView.TargetItem.Description);

            var translate = new Vector2(itemView.Root.layout.position.x + itemView.Root.layout.width / 2, 0);
            _itemInfoView.SetTransform(translate);
        }

        public void HideItemInfo(ItemView itemView) => _itemInfoView.Hide();
    }
}