namespace Impromptu.DesignPattern.MVP
{
    /*
     * Controller/Presenter:
     * Receives input and initiates a response by making calls on model and View objects.
     */
    internal class Presenter
    {
        private readonly Model m_model;
        private readonly IView m_view;

        protected Presenter(IView view, Model model)
        {
            m_view = view;
            m_model = model;

            m_view.Subscribe(FunctionPointer);
        }

        protected virtual void FunctionPointer()
        {
            m_model.Operation();
        }
    }
}
