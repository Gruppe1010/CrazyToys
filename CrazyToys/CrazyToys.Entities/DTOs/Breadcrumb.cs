namespace CrazyToys.Entities.DTOs
{
    public class Breadcrumb
    {
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public object Id { get; set; }
        public string Title { get; set; }

        public Breadcrumb(string area, string controller, string action, string title, object id) : this(area, controller, action, title)
        {
            Id = id;
        }

        public Breadcrumb(string area, string controller, string action, string title)
        {
            Area = area;
            Controller = controller;
            Action = action;
            Title = title;
        }
    }
}
