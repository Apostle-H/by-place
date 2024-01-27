using PointNClick.Cursor.Sensitive;

namespace PointNClick.Cursor.Manager
{
    public interface ICursorManager
    {
        public void AddSensitive(ICursorSensitive sensitive);
        public void RemoveSensitive(ICursorSensitive sensitive);
    }
}