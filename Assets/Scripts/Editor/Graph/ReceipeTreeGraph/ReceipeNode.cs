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

            // Result의 ToString() 값으로 노드 제목 설정
            title = data.Result != null ? data.Result.GetType().ToString() : "Unknown Recipe";

            // Ingredients 리스트 UI 추가
            var ingredientsContainer = new VisualElement { name = "Ingredients" };
            ingredientsContainer.Add(new Label("Ingredients"));
            foreach (var ingredient in receipeData.Ingredients)
            {
                var ingredientField = new TextField { value = ingredient.ToString() };
                ingredientField.SetEnabled(false); // Ingredient를 읽기 전용으로 설정
                ingredientsContainer.Add(ingredientField);
            }
            mainContainer.Add(ingredientsContainer);

            // Result 필드 UI 추가
            var resultField = new TextField("Result") { value = receipeData.Result.ToString() };
            resultField.SetEnabled(false); // Result를 읽기 전용으로 설정
            mainContainer.Add(resultField);

            // CookingSec 필드 UI 추가
            var cookingSecField = new FloatField("Cooking Time (sec)") { value = receipeData.CookingSec };
            cookingSecField.SetEnabled(false);
            mainContainer.Add(cookingSecField);

            // CookwareType 필드 UI 추가
            var typeField = new EnumField("Cookware Type", receipeData.type);
            typeField.SetEnabled(false);
            mainContainer.Add(typeField);

            // Input 포트 추가
            inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
            inputPort.portName = "Input";
            inputContainer.Add(inputPort);

            // Output 포트 추가
            outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputPort.portName = "Output";
            outputContainer.Add(outputPort);

            RefreshExpandedState();
            RefreshPorts();
        }
    }
}

#endif