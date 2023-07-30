

namespace KuranX.App.Core.Classes
{
    public class Tasks
    {
        public int tasksId { get; set; }
        public int missonsId { get; set; } = 0;
        public int missonsTime { get; set; } = 0;
        public int missonsRepeart { get; set; } = 0;
        public string missonsType { get; set; } = "Default";
        public string missonsColor { get; set; } = "#FFFFFF";
    }
}