using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Teste : MonoBehaviour {

    UIGameData gameData;

    bool _canSwitch = true;

    private void Start() {
        gameData = FindObjectOfType<UIGameData>();

        HandleButtonsEvents();
        PainelStartPosition();
        HandleButton();
    }

    // Create button events
    private void HandleButtonsEvents() {

        foreach (CustomData data in gameData.uiData) {
            foreach (ButtonData buttons in data.buttonDatas) {
                buttons.button.onClick = new Button.ButtonClickedEvent();
            }
        }

    }

    private void HandleButton() {

        // Adiciona uma função a cada botão dentro da lista 
        foreach (CustomData data in gameData.uiData) {
            foreach (ButtonData buttons in data.buttonDatas) {
                buttons.button.onClick.AddListener(() => SwitchPainel(buttons.buttonIndex));
            }
        }

        // Adiciona uma função ao botão (Start Game)
        gameData.StartButton.onClick.AddListener(() => StartGame());


        // Adiciona uma função ao botão (Exit Game)
        gameData.ExitButton.onClick.AddListener(() => ExitGame());
    }
    private void SwitchPainel(int painelIndex) {

        if (gameData.uiData[painelIndex].inCenter == true) return;

        if (_canSwitch) {
            _canSwitch = false;

            // Percorrer a lista uiData 
            for (int i = 0; i < gameData.uiData.Count; i++) {
                // Se encontrar o objeto da lista com a booleana inCenter = true
                if (gameData.uiData[i].inCenter) {

                    ChangeAnimation(i, painelIndex);
                }
            }
        }
    }

    private void ChangeAnimation(int index, int painelIndex) {
        switch (gameData.uiData[index]._ChangeAnimation) {
            case CustomData.ChangeAnimation.Position:
                TesteIndex customDataPosition = gameData.customDataDictionary[CustomDataEnum.CustomDataPosition];
                PositionAnimation(index, customDataPosition);
                PositionAnimationIn(painelIndex, customDataPosition);
                break;
            case CustomData.ChangeAnimation.Scale:
                TesteIndex customDataRotation = gameData.customDataDictionary[CustomDataEnum.CustomDataRotation];
                ScaleAnimation(index, customDataRotation);
                ScaleAnimationIn(painelIndex, customDataRotation);
                break;
        }
    }

    private void PositionAnimation(int index, TesteIndex customDataPosition) {
        // Faz a animação do painel de sair do canvas e ponho a booleana inCenter = false
        gameData.uiData[index].painel.DOAnchorPos(new Vector3(customDataPosition.painelAnimOut.x, customDataPosition.painelAnimOut.y, 0), gameData.uiData[index].duration).SetEase(gameData.uiData[index].animationStyle);
        gameData.uiData[index].inCenter = false;
    }

    private void PositionAnimationIn(int painelIndex, TesteIndex customDataPosition) {
        // Faz a animação do painel escolhido ao entrar no canvas e ponho a booleana deste painel inCenter = true
        gameData.uiData[painelIndex].painel.DOAnchorPos(new Vector3(customDataPosition.painelAnimIn.x, customDataPosition.painelAnimIn.y, 0), gameData.uiData[painelIndex].duration).SetEase(gameData.uiData[painelIndex].animationStyle).OnComplete(() => {
            gameData.uiData[painelIndex].inCenter = true;
            _canSwitch = true;
        });
    }

    private void ScaleAnimation(int index, TesteIndex customDataPosition) {
        // Faz a animação do painel de sair do canvas e ponho a booleana inCenter = false
        gameData.uiData[index].painel.DOScale(new Vector3(customDataPosition.painelAnimOut.x, customDataPosition.painelAnimOut.y, 0), gameData.uiData[index].duration).SetEase(gameData.uiData[index].animationStyle);
        gameData.uiData[index].inCenter = false;
    }
    private void ScaleAnimationIn(int painelIndex, TesteIndex customDataPosition) {
        // Faz a animação do painel escolhido ao entrar no canvas e ponho a booleana deste painel inCenter = true
        gameData.uiData[painelIndex].painel.DOScale(new Vector3(customDataPosition.painelAnimIn.x, customDataPosition.painelAnimIn.y, 0), gameData.uiData[painelIndex].duration).SetEase(gameData.uiData[painelIndex].animationStyle).OnComplete(() => {
            gameData.uiData[painelIndex].inCenter = true;
            _canSwitch = true;
        });
    }

    private void PainelStartPosition() {
        // Posicionar todos os paineis na posição indicada ao começar o jogo 

        for (int i = 0; i < gameData.uiData.Count; i++) {
            gameData.uiData[i].painel.localPosition = gameData.uiData[i].testeIndex.painelStartPosition;
        }
        
    }
    private void StartGame() {
        SceneManager.LoadSceneAsync("Dasher");
    }

    private void ExitGame() {
        Application.Quit();
    }
}
