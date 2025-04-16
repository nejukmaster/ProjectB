#if UNITY_EDITOR

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectB
{
    public class ReceipeNode : Node
    {
        public Receipe receipeData;

        public Port inputPort;
        public Port outputPort;

        public ReceipeNode(Receipe data)
        {
            this.receipeData = data;

            // Result�� ToString() ������ ��� ���� ����
            title = data.Result != null ? data.Result.GetType().ToString() : "Unknown Recipe";

            // Ingredients ����Ʈ UI �߰�
            var ingredientsContainer = new VisualElement { name = "Ingredients" };
            ingredientsContainer.Add(new Label("Ingredients"));
            foreach (var ingredient in receipeData.Ingredients)
            {
                var ingredientField = new TextField { value = ingredient.ToString() };
                ingredientField.SetEnabled(false); // Ingredient�� �б� �������� ����
                ingredientsContainer.Add(ingredientField);
            }
            mainContainer.Add(ingredientsContainer);

            // Result �ʵ� UI �߰�
            var resultField = new TextField("Result") { value = receipeData.Result.ToString() };
            resultField.SetEnabled(false); // Result�� �б� �������� ����
            mainContainer.Add(resultField);

            // CookingSec �ʵ� UI �߰�
            var cookingSecField = new FloatField("Cooking Time (sec)") { value = receipeData.CookingSec };
            cookingSecField.SetEnabled(false);
            mainContainer.Add(cookingSecField);

            // CookwareType �ʵ� UI �߰�
            var typeField = new EnumField("Cookware Type", receipeData.type);
            typeField.SetEnabled(false);
            mainContainer.Add(typeField);

            // Input ��Ʈ �߰�
            inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
            inputPort.portName = "Input";
            inputContainer.Add(inputPort);

            // Output ��Ʈ �߰�
            outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputPort.portName = "Output";
            outputContainer.Add(outputPort);

            RefreshExpandedState();
            RefreshPorts();
        }
    }
}

#endif