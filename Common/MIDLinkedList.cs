using System;
using System.Collections;

namespace MIDRetail.Common
{
	public class MIDLinkedListNode<TValue>
	{
		internal TValue _data;
		internal MIDLinkedListNode<TValue> _previous;
		internal MIDLinkedListNode<TValue> _next;

		public MIDLinkedListNode(TValue aData)
		{
			_data = aData;
			_previous = null;
			_next = null;
		}

		public TValue Data
		{
			get
			{
				return _data;
			}
		}

		public MIDLinkedListNode<TValue> Previous
		{
			get
			{
				return _previous;
			}
		}

		public MIDLinkedListNode<TValue> Next
		{
			get
			{
				return _next;
			}
		}

		internal void InsertBefore(MIDLinkedListNode<TValue> aBeforeNode)
		{
			try
			{
				_next = aBeforeNode;
				_previous = aBeforeNode._previous;
				aBeforeNode._previous = this;

				if (_previous != null)
				{
					_previous._next = this;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		internal void InsertAfter(MIDLinkedListNode<TValue> aAfterNode)
		{
			try
			{
				_previous = aAfterNode;
				_next = aAfterNode._next;
				aAfterNode._next = this;

				if (_next != null)
				{
					_next._previous = this;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		internal void Remove()
		{
			try
			{
				if (_previous != null)
				{
					_previous._next = this._next;
				}

				if (_next != null)
				{
					_next._previous = this._previous;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	public class MIDLinkedList<TValue>
	{
		private MIDLinkedListNode<TValue> _beginning;
		private MIDLinkedListNode<TValue> _end;

		public MIDLinkedList()
		{
			_beginning = null;
			_end = null;
		}

		public MIDLinkedListNode<TValue> Beginning
		{
			get
			{
				return _beginning;
			}
		}

		public MIDLinkedListNode<TValue> End
		{
			get
			{
				return _end;
			}
		}

		public void Clear()
		{
			try
			{
				_beginning = null;
				_end = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public MIDLinkedListNode<TValue> InsertNodeAtBeginning(MIDLinkedListNode<TValue> aNode)
		{
			try
			{
				if (_beginning != null)
				{
					aNode.InsertBefore(_beginning);
				}

				if (_end == null)
				{
					_end = aNode;
				}

				_beginning = aNode;

				return aNode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public MIDLinkedListNode<TValue> InsertNodeAtEnd(MIDLinkedListNode<TValue> aNode)
		{
			try
			{
				if (_end != null)
				{
					aNode.InsertAfter(_end);
				}

				if (_beginning == null)
				{
					_beginning = aNode;
				}

				_end = aNode;

				return aNode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public MIDLinkedListNode<TValue> InsertNodeBefore(MIDLinkedListNode<TValue> aInsertNode, MIDLinkedListNode<TValue> aBeforeNode)
		{
			try
			{
				aInsertNode.InsertBefore(aBeforeNode);

				if (aInsertNode.Previous == null)
				{
					_beginning = aInsertNode;
				}

				if (aInsertNode.Next == null)
				{
					_end = aInsertNode;
				}

				return aInsertNode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public MIDLinkedListNode<TValue> InsertNodeAfter(MIDLinkedListNode<TValue> aInsertNode, MIDLinkedListNode<TValue> aAfterNode)
		{
			try
			{
				aInsertNode.InsertAfter(aAfterNode);

				if (aInsertNode.Previous == null)
				{
					_beginning = aInsertNode;
				}

				if (aInsertNode.Next == null)
				{
					_end = aInsertNode;
				}

				return aInsertNode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public MIDLinkedListNode<TValue> MoveNodeToBeginning(MIDLinkedListNode<TValue> aNode)
		{
			try
			{
				RemoveNode(aNode);

				if (_beginning != null)
				{
					aNode.InsertBefore(_beginning);
				}

				if (_end == null)
				{
					_end = aNode;
				}

				_beginning = aNode;

				return aNode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public MIDLinkedListNode<TValue> MoveNodeToEnd(MIDLinkedListNode<TValue> aNode)
		{
			try
			{
				RemoveNode(aNode);

				if (_end != null)
				{
					aNode.InsertAfter(_end);
				}

				if (_beginning == null)
				{
					_beginning = aNode;
				}

				_end = aNode;

				return aNode;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ArrayList RemoveLastNodes(int aNumNodes)
		{
			ArrayList nodeList;
			MIDLinkedListNode<TValue> node;
			int count;

			try
			{
				nodeList = new ArrayList();
				node = _end;
				count = 0;

				while (node != null && count < aNumNodes)
				{
					RemoveNode(node);
					nodeList.Add(node);
					node = node.Previous;
					count++;
				}

				return nodeList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void RemoveNode(MIDLinkedListNode<TValue> aNode)
		{
			try
			{
				if (aNode.Previous == null)
				{
					_beginning = aNode.Next;
				}

				if (aNode.Next == null)
				{
					_end = aNode.Previous;
				}

				aNode.Remove();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
