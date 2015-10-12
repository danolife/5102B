using System;

namespace HuffmanLibrary
{
    public class Noeud
    {
        private byte letter;
        private int value;
        private Noeud leftChild;
        private Noeud rightChild;
        private Noeud parent;
        private bool isRight;

        public Noeud()
        {
            letter = 0;
            value = 0;
            leftChild = null;
            rightChild = null;
            parent = null;
            isRight = false;
        }
        public Noeud(int pValue, byte pLetter)
        {
            letter = pLetter;
            value = pValue;
            leftChild = null;
            rightChild = null;
            parent = null;
            isRight = false;
        }
        public Noeud(int pValue, Noeud pLeft, Noeud pRight)
        {
            letter = 0;
            value = pValue;
            leftChild = pLeft;
            rightChild = pRight;
            parent = null;
            isRight = false;
        }

        /*public static Noeud operator= (Noeud n){
            Noeud copie = 
            this.letter = n.letter;
            this.value = n.value;
            this.leftChild = n.leftChild;
            this.rightChild = n.rightChild;
            this.parent = n.parent;
            this.isRight = n.isRight;
        }*/

        public byte getLetter()
        {
            return this.letter;
        }
        public int getValue()
        {
            return this.value;
        }
        public Noeud getLeftChild()
        {
            return this.leftChild;
        }
        public Noeud getRightChild()
        {
            return this.rightChild;
        }
        public void setLeftChild(Noeud pLeft)
        {
            this.leftChild = pLeft;
        }
        public void setRightChild(Noeud pRight)
        {
            this.rightChild = pRight;
        }
        public Noeud getParent()
        {
            return this.parent;
        }
        public void setParent(Noeud pParent)
        {
            this.parent = pParent;
        }
        public bool getIsRight()
        {
            return this.isRight;
        }
        public void setIsRight(bool pIsRight)
        {
            this.isRight = pIsRight;
        }

    }
}
