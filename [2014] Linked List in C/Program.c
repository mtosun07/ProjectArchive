#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#ifdef _WIN32
#define MAX_PATH_LEN 260
#else
#include <limits.h>
#define MAX_PATH_LEN PATH_MAX
#endif

typedef struct Node {
    char* Value;
    int Count;
    struct Node* Next;
    struct Node* Previous;
} Node;

typedef struct LinkedList {
    Node* Head;
    Node* Tail;
    int Length;
} LinkedList;

void AddLast(LinkedList* list, const char* value) {
    Node* node = list->Head;
    while (node != NULL) {
        if (strcmp(node->Value, value) == 0) {
            node->Count++;
            return;
        }
        node = node->Next;
    }

    Node* newNode = (Node*)malloc(sizeof(Node));
    newNode->Value = (char*)malloc(strlen(value) + 1);
    strcpy(newNode->Value, value);
    newNode->Count = 1;
    newNode->Next = NULL;
    newNode->Previous = list->Tail;

    if (list->Tail)
        list->Tail->Next = newNode;
    else
        list->Head = newNode;

    list->Tail = newNode;
    list->Length++;
}

void Print(LinkedList* list) {
    Node* node = list->Head;
    while (node != NULL) {
        printf("%s : %d\n", node->Value, node->Count);
        node = node->Next;
    }
}

void PrintSorted(LinkedList* list) {
    if (list->Length == 0) return;

    Node** nodes = (Node**)malloc(sizeof(Node*) * list->Length);
    Node* node = list->Head;
    int idx = 0;
    while (node != NULL) {
        nodes[idx++] = node;
        node = node->Next;
    }

    for (int i = 0; i < list->Length - 1; i++) {
        for (int j = 0; j < list->Length - i - 1; j++) {
            if (nodes[j]->Count < nodes[j + 1]->Count) {
                Node* tmp = nodes[j];
                nodes[j] = nodes[j + 1];
                nodes[j + 1] = tmp;
            }
        }
    }

    for (int i = 0; i < list->Length; i++) {
        printf("%s : %d\n", nodes[i]->Value, nodes[i]->Count);
    }

    free(nodes);
}

void LinkedListFree(LinkedList* list) {
    Node* node = list->Head;
    while (node != NULL) {
        Node* tmp = node;
        node = node->Next;
        free(tmp->Value);
        free(tmp);
    }
}

int isSpecial(char c) {
    return !((c >= 'a' && c <= 'z') ||
        (c >= 'A' && c <= 'Z') ||
        (c >= '0' && c <= '9'));
}

void SplitAndAdd(LinkedList* list, char* buffer) {
    int i = 0;
    while (buffer[i] != '\0') {
        if (buffer[i] == ' ' || buffer[i] == '\t' || buffer[i] == '\n') {
            i++;
            continue;
        }

        int start = i;
        char* token;

        if (isSpecial(buffer[i])) {
            token = (char*)malloc(2);
            token[0] = buffer[i];
            token[1] = '\0';
            AddLast(list, token);
            free(token);
            i++;
        }
        else {
            while (buffer[i] != '\0' && !isSpecial(buffer[i]) &&
                buffer[i] != ' ' && buffer[i] != '\t' && buffer[i] != '\n') {
                i++;
            }
            int len = i - start;
            token = (char*)malloc(len + 1);
            strncpy(token, buffer + start, len);
            token[len] = '\0';
            AddLast(list, token);
            free(token);
        }
    }
}

int main() {
    LinkedList list;
    list.Head = list.Tail = NULL;
    list.Length = 0;

    char path[MAX_PATH_LEN];
    printf("Dosya yolunu giriniz >: ");
    fgets(path, sizeof(path), stdin);
    path[strcspn(path, "\r\n")] = '\0';

    FILE* oku = fopen(path, "r");
    if (!oku) {
        perror("Dosya acilamadi!");
        getchar();
        return 1;
    }

    char buffer[4096];
    while (fgets(buffer, sizeof(buffer), oku)) {
        SplitAndAdd(&list, buffer);
    }
    fclose(oku);

    PrintSorted(&list);
    LinkedListFree(&list);

    printf("\nCikmak icin Enter'a basiniz...");
    getchar();
    return 0;
}
