using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Algorithm : MonoBehaviour
{

    //-------------------------------------------------------------------------------------------------------------
    public void Algorithm0001()
    {
        int[] nums = new[] { 2, 7, 11, 15 };
        int target = 9;
        UnityEngine.Debug.Log(TwoSum(nums,target)); //[0,1] 
    }
    
    
    //方法三：一遍哈希表
    public int[] TwoSum(int[] nums, int target)
    {
        Dictionary<int, int> kvs = new Dictionary<int, int>(); //创建一个空的哈希表 kvs，用来存储数组元素和它们的索引。
        for (int i = 0; i < nums.Length; i++) //遍历数组 nums，对于每个元素 nums[i]，计算与目标值的差值 complement = target - nums[i]。
        {
            int complement = target - nums[i];
            if (kvs.ContainsKey(complement) && kvs[complement] != i) //检查哈希表 kvs 中是否存在键为 complement 的元素，且该元素的索引不等于当前遍历的索引 i。
            {
                return new int[] { i, kvs[complement] };
            }
            //需要对重复值进行判断,若结果包含了重复值，则已经被上面给return了；所以此处对于重复值直接忽略
            if (!kvs.ContainsKey(nums[i]))
            {
                kvs.Add(nums[i], i);
            }
        }
        return new int[] { 0, 0 };
    }
    
    //-------------------------------------------------------------------------------------------------------------
    //2. 两数相加
    //给你两个 非空 的链表，表示两个非负的整数。它们每位数字都是按照 逆序 的方式存储的，并且每个节点只能存储 一位 数字。
    //请你将两个数相加，并以相同形式返回一个表示和的链表。
    //你可以假设除了数字 0 之外，这两个数都不会以 0 开头。
    
    //输入：l1 = [9,9,9,9,9,9,9], l2 = [9,9,9,9]
    //输出：[8,9,9,9,0,0,0,1]
    public void Algorithm0002()
    {
        int[] arr1 = { 9,9,9,9,9,9,9};
        ListNode list1 = ArrayToListNode(arr1);

        int[] arr2 = { 9, 9, 9, 9 };
        ListNode list2 = ArrayToListNode(arr2);
        UnityEngine.Debug.Log(AddTwoNumbers(list1,list2)); //[0,1] 
    }
    
    
    public ListNode ArrayToListNode(int[] arr) {
        if (arr == null || arr.Length == 0) {
            return null;
        }

        ListNode head = new ListNode(arr[0]);
        ListNode current = head;

        for (int i = 1; i < arr.Length; i++) { //从前往后
            current.next = new ListNode(arr[i]);
            current = current.next;
        }

        return head;
    }
    

    
    public class ListNode {
        public int val;
        public ListNode next;
        public ListNode(int val=0, ListNode next=null) {
            this.val = val;
            this.next = next;
        }
    }
    
    public ListNode AddTwoNumbers(ListNode l1, ListNode l2) {
        ListNode head = null, tail = null; //创建两个变量 head 和 tail，用于构建结果链表，开始时它们都为 null。
        int carry = 0; //创建一个变量 carry，用于保存进位的值，初始值为 0。
        while (l1 != null || l2 != null) { //进入一个循环，只要 l1 或 l2 中任一链表还有节点（未遍历完），就执行以下操作：
            int n1 = l1 != null ? l1.val : 0;
            int n2 = l2 != null ? l2.val : 0;
            int sum = n1 + n2 + carry; //计算当前位置的值，分别从 l1 和 l2 中获取节点的值，并加上上一次的进位值。

            int current = sum % 10; //将这个值取模 10，将其作为新节点的值，并创建一个新的节点。
            if (head == null) { //如果 head 为 null，表示这是结果链表的第一个节点，因此将 head 和 tail 都指向这个新节点。
                head = tail = new ListNode(current);
            } else {
                tail.next = new ListNode(current); //否则，将新节点连接到 tail 的后面，然后更新 tail 为新节点。
                tail = tail.next;
            }
            carry = sum / 10; 
            
            
            
            //分别将 l1 和 l2 向前移动一个节点（如果它们还有节点的话）。
            if (l1 != null) {
                l1 = l1.next;
            }
            if (l2 != null) {
                l2 = l2.next;
            }
        }
        
        
        //检查是否还有进位值 carry 大于 0，如果有，创建一个额外的节点，将进位值放入该节点，并连接到结果链表的末尾。
        if (carry > 0) {
            tail.next = new ListNode(carry);
        }
        
        //返回结果链表 head，它包含了两个输入链表相加的结果。
        return head;    
    }

    
    //-------------------------------------------------------------------------------------------------------------
    //3. 无重复字符的最长子串
    //给定一个字符串 s ，请你找出其中不含有重复字符的 最长子串 的长度。
    // 输入: s = "abcabcbb"
    // 输出: 3 
    // 解释: 因为无重复字符的最长子串是 "abc"，所以其长度为 3。
    public int LengthOfLongestSubstring(string s) {
        HashSet<char> letter = new HashSet<char>();// 哈希集合，记录每个字符是否出现过
        
        int left = 0,right = 0;//left 表示当前不重复子串的起始位置，right 表示当前不重复子串的结束位置。
        
        int length = s.Length; //获取输入字符串的长度，用 length 变量保存。
        int count = 0,max = 0;//count记录每次指针移动后的子串长度 max 用于记录最长不重复子串的长度，初始值都为 0
        
        
        while(right < length) //条件是 right 指针小于字符串的长度，即尚未遍历完整个字符串。
        {
            if(!letter.Contains(s[right]))//右指针字符未重复
            {
                letter.Add(s[right]);//将该字符添加进集合
                right++;//右指针继续右移
                count++;
            }
            else//右指针字符重复，左指针开始右移，直到不含重复字符（即左指针移动到重复字符(左)的右边一位）
            { 
                letter.Remove(s[left]);//去除集合中当前左指针字符
                left++;//左指针右移
                count--;
            }
            max = Math.Max(max,count);
        }
        return max;
    }

    
    //4. 寻找两个正序数组的中位数 
    //给定两个大小分别为 m 和 n 的正序（从小到大）数组 nums1 和 nums2。请你找出并返回这两个正序数组的 中位数 。
    // 输入：nums1 = [1,3], nums2 = [2]
    // 输出：2.00000
    // 解释：合并数组 = [1,2,3] ，中位数 2
    public double FindMedianSortedArrays(int[] nums1, int[] nums2)
    {
        int m = nums1.Length;
        int n = nums2.Length;
        int len = m + n;
        var resultIndex = len / 2;
        List<int> list = new List<int>(nums1);
        list.AddRange(nums2);
        list.Sort();
        if (len % 2 == 0)
        {
            return (list[resultIndex - 1] + list[resultIndex]) / 2.0;
        }
        else
        {
            return list[resultIndex];
        }
    }
    
    public double FindMedianSortedArrays2(int[] nums1, int[] nums2)
    {
        // nums1 与 nums2 有序添加到list中
        List<int> list = new List<int>();
        int i = 0, j = 0;
        int m = nums1.Length;
        int n = nums2.Length;
        int len = m + n;
        var resultIndex = len / 2;

        while (i < m && j < n)
        {
            if (nums1[i] < nums2[j]) //将较小的值添加到 list 中，并相应地移动指针 i 或 j。
                list.Add(nums1[i++]);
            else
                list.Add(nums2[j++]);
        }
        
        //如果有一个数组中的元素已经全部添加到 list 中，但另一个数组还有剩余元素，就使用两个 while 循环分别将剩余的元素添加到 list 中。
        while (i < m) list.Add(nums1[i++]);
        while (j < n) list.Add(nums2[j++]);

        if (len % 2 == 0)
        {
            return (list[resultIndex - 1] + list[resultIndex]) / 2.0;
        }
        else
        {
            return list[resultIndex];
        }
    }
    
    
    // 5. 最长回文子串
    // 给你一个字符串 s，找到 s 中最长的回文子串。
    //如果字符串的反序与原始字符串相同，则该字符串称为回文字符串。
    // 输入：s = "babad"
    // 输出："bab"
    // 解释："aba" 同样是符合题意的答案。
    public string LongestPalindrome(string s)
    {
        string result = "";
        int n = s.Length;
        int end = 2 * n - 1; // end 为字符串长度的两倍减一。
        for (int i = 0; i < end; i ++)
        {
            double mid = i / 2.0;
            int p = (int)(Math.Floor(mid));
            int q = (int)(Math.Ceiling(mid));
            while (p >= 0 && q < n)
            {
                if (s[p] != s[q]) break;
                p--; q++;
            }
            int len = q - p - 1;
            if (len > result.Length)
                result = s.Substring(p + 1, len);
        }
        return result;
    }
    
    //6. N 字形变换
    //将一个给定字符串 s 根据给定的行数 numRows ，以从上往下、从左到右进行 Z 字形排列。
    //比如输入字符串为 "PAYPALISHIRING" 行数为 3 时，排列如下：
    // P   A   H   N
    // A P L S I I G 
    // Y   I   R
    //之后，你的输出需要从左往右逐行读取，产生出一个新的字符串，比如："PAHNAPLSIIGYIR"。
    // 输入：s = "PAYPALISHIRING", numRows = 4
    // 输出："PINALSIGYAHRPI"
    // 解释：
    // P       I    N
    // A   L   S  I G
    // Y A     H R
    // P       I
    public string Convert(string s, int numRows)
    {
        if (numRows == 1)
        {
            return s;
        }

        List<List<char>> rows = new List<List<char>>();
        //创建一个列表 rows，用于存储 Z 字形变换后的字符，列表中包含 numRows 个子列表。
        for (int i = 0; i < s.Length && i < numRows; i++)
        {
            rows.Add(new List<char>());
        }
        
        bool isdown = false;
        int row = 0;
        //遍历字符串 s 中的字符，将字符逐个添加到 rows 中的对应子列表中，同时维护一个变量 row 表示当前行数。
        foreach (var c in s)
        {
            rows[row].Add(c);
            //使用 isdown 变量来确定字符是向下移动还是向上移动。当 row 处于第一行或最后一行时，切换移动方向。
            if (row == 0 || row == numRows - 1)
            {
                isdown = !isdown;
            }
            row += isdown ? 1 : -1;
        }

        //遍历 rows 中的每个子列表，并将字符连接起来，形成 Z 字形变换后的字符串。
        StringBuilder sb = new StringBuilder();
        foreach (var l in rows)
        {
            sb.Append(new string(l.ToArray()));
        }

        return sb.ToString();
    }

    
    //7. 整数反转
    //给你一个 32 位的有符号整数 x ，返回将 x 中的数字部分反转后的结果。
    //如果反转后整数超过 32 位的有符号整数的范围 [−231,  231 − 1] ，就返回 0。
    //假设环境不允许存储 64 位整数（有符号或无符号）。
    //输入：x = 123
    //输出：321
    public int Reverse(int x) {
        int rev = 0; //初始化一个变量 rev 为 0，用于存储反转后的整数。
        while (x != 0) { //进入一个循环，该循环会一直执行直到输入整数 x 变为 0。
            
            
            //在循环内部，首先检查 rev 是否已经超出了整数的范围，即是否小于 int.MinValue / 10 或大于 int.MaxValue / 10，如果是的话，就返回 0。这是为了避免反转后的整数溢出。
            if (rev < int.MinValue / 10 || rev > int.MaxValue / 10) {
                return 0;
            }
            
            //获取 x 的最后一位数字，通过取模运算 x % 10 得到，并将其存储在变量 digit 中。
            int digit = x % 10;
            
            x /= 10; //将 x 除以 10，以去掉最后一位数字。
            rev = rev * 10 + digit; //将 rev 左移一位（相当于乘以 10）并加上 digit，将新的数字追加到 rev 的末尾。
        }
        return rev;
    }
    
    //8. 字符串转换整数 (atoi)
    //请你来实现一个 myAtoi(string s) 函数，使其能将字符串转换成一个 32 位有符号整数（类似 C/C++ 中的 atoi 函数）。
    // 函数 myAtoi(string s) 的算法如下：
    // 读入字符串并丢弃无用的前导空格
    //     检查下一个字符（假设还未到字符末尾）为正还是负号，读取该字符（如果有）。 确定最终结果是负数还是正数。 如果两者都不存在，则假定结果为正。
    // 读入下一个字符，直到到达下一个非数字字符或到达输入的结尾。字符串的其余部分将被忽略。
    // 将前面步骤读入的这些数字转换为整数（即，"123" -> 123， "0032" -> 32）。如果没有读入数字，则整数为 0 。必要时更改符号（从步骤 2 开始）。
    // 如果整数数超过 32 位有符号整数范围 [−231,  231 − 1] ，需要截断这个整数，使其保持在这个范围内。具体来说，小于 −231 的整数应该被固定为 −231 ，大于 231 − 1 的整数应该被固定为 231 − 1 。
    // 返回整数作为最终结果。
    // 注意：
    // 本题中的空白字符只包括空格字符 ' ' 。
    // 除前导空格或数字后的其余字符串外，请勿忽略 任何其他字符。
    // 输入：s = "   -42"
    // 输出：-42
    // 解释：
    // 第 1 步："   -42"（读入前导空格，但忽视掉）
    // 第 2 步："   -42"（读入 '-' 字符，所以结果应该是负数）
    // 第 3 步："   -42"（读入 "42"）
    // 解析得到整数 -42 。
    // 由于 "-42" 在范围 [-231, 231 - 1] 内，最终结果为 -42 。
    public int MyAtoi(string s) {
        int sum=0;
        //清空空格
        s=s.Trim();
        if(s.Length==0) return 0;
        
        
        //如果首位不是数字且不是‘-’且不为‘+’直接返回0
        if(!char.IsDigit(s[0]) && s[0]!='-' && s[0]!='+') return 0;
        
        
        
        //判断最后一位数字截取
        for(int i=1;i<s.Length;i++)
        {
            if(!char.IsDigit(s[i]))
            {
                s=s.Substring(0,i);
                break;
            }
        }
        
        //判断是否只剩下符号
        if (s == "-" || s == "+" || s == "+-" || s == "-+") return 0;
        
        
        //s是要转换的字符串，i 是转换的结果。
        if(int.TryParse(s,out int x))
        {
            sum=x;
        }
        else
        {
            if(s.Contains('-')) sum=int.MinValue;
            else sum=int.MaxValue;
        }
        return sum;
    }

    //9. 回文数
    //给你一个整数 x ，如果 x 是一个回文整数，返回 true ；否则，返回 false 。
    //回文数是指正序（从左向右）和倒序（从右向左）读都是一样的整数。
    //例如，121 是回文，而 123 不是。
    // 输入：x = -121
    // 输出：false
    // 解释：从左向右读, 为 -121 。 从右向左读, 为 121- 。因此它不是一个回文数。
    public bool IsPalindrome(int x) {
        // 特殊情况：
        // 如上所述，当 x < 0 时，x 不是回文数。
        // 同样地，如果数字的最后一位是 0，为了使该数字为回文，
        // 则其第一位数字也应该是 0
        // 只有 0 满足这一属性
        if (x < 0 || (x % 10 == 0 && x != 0)) {
            return false;
        }

        int revertedNumber = 0;
        while (x > revertedNumber) {
            revertedNumber = revertedNumber * 10 + x % 10;
            x /= 10;
        }

        // 当数字长度为奇数时，我们可以通过 revertedNumber/10 去除处于中位的数字。
        // 例如，当输入为 12321 时，在 while 循环的末尾我们可以得到 x = 12，revertedNumber = 123，
        // 由于处于中位的数字不影响回文（它总是与自己相等），所以我们可以简单地将其去除。
        return x == revertedNumber || x == revertedNumber / 10;
    }
    
    //10. 正则表达式匹配
    // 给你一个字符串 s 和一个字符规律 p，请你来实现一个支持 '.' 和 '*' 的正则表达式匹配。
    // '.' 匹配任意单个字符
    // '*' 匹配零个或多个前面的那一个元素
    // 所谓匹配，是要涵盖 整个 字符串 s的，而不是部分字符串。
    // 输入：s = "aa", p = "a"
    // 输出：false
    // 解释："a" 无法匹配 "aa" 整个字符串。
    public bool IsMatch(string s, string p)
    {
        bool[,] dp = new bool[s.Length + 1, p.Length + 1];  //
        dp[s.Length, p.Length] = true; // dp[s.Length, p.Length] 设置为 true，表示空字符串和空正则表达式可以匹配
        for (int i = s.Length; i >= 0; i--) //从字符串的末尾开始往前遍历（i = s.Length; i >= 0），
        {
            for (int j = p.Length - 1; j >= 0; j--) //再从正则表达式的末尾的前一个字符开始往前遍历（j = p.Length - 1; j >= 0）。
            {
                
                // 检查当前字符是否匹配，包括字符相同或正则中的'.'
                bool first_match = (i < s.Length && (p[j] == s[i] || p[j] == '.'));
                
                //如果正则表达式的下一个字符是 '*'：
                if (j + 1 < p.Length && p[j + 1] == '*')
                {
                    // 有两种可能性：
                    // A. 跳过正则中的字符和'*'（前一个字符出现0次）
                    // B. 匹配前一个字符的一个或多个出现，并移动到字符串s的下一个字符

                    dp[i, j] = dp[i, j + 2]    //A
                               || first_match && dp[i + 1, j]; //B
                }
                //如果正则表达式的下一个字符不是 '*'，
                else
                {
                    // 匹配当前字符并移动到字符串s和正则表达式p的下一个字符
                    dp[i, j] = first_match && dp[i + 1, j + 1]; // C
                }
            }
        }
        return dp[0, 0];
    }


    //11. 盛最多水的容器
    //给定一个长度为 n 的整数数组 height 。有 n 条垂线，第 i 条线的两个端点是 (i, 0) 和 (i, height[i]) 。
    //找出其中的两条线，使得它们与 x 轴共同构成的容器可以容纳最多的水。
    //返回容器可以储存的最大水量。
    //说明：你不能倾斜容器。
    //输入：[1,8,6,2,5,4,8,3,7]
    //输出：49 
    //解释：图中垂直线代表输入数组 [1,8,6,2,5,4,8,3,7]。在此情况下，容器能够容纳水（表示为蓝色部分）的最大值为 49。
    public int MaxArea(int[] height) {
        int maxAmount = 0;              // 用于存储最大容水量的变量
        int left = 0, right = height.Length - 1; // 左右两个指针，分别指向数组的首尾

        while (left < right) {
            if (height[left] <= height[right]) {
                // 计算当前容器的容水量，取较短的边乘以两个指针之间的距离
                int amount = (right - left) * height[left];
                maxAmount = Math.Max(maxAmount, amount); // 更新最大容水量
                left++; // 移动左指针，寻找更高的容器边界
            } else {
                int amount = (right - left) * height[right];
                maxAmount = Math.Max(maxAmount, amount); // 更新最大容水量
                right--; // 移动右指针，寻找更高的容器边界
            }
        }
        return maxAmount; // 返回最大容水量
    }

    //12. 整数转罗马数字
    // 罗马数字包含以下七种字符： I， V， X， L，C，D 和 M。

    // 字符          数值
    // I             1
    // V             5
    // X             10
    // L             50
    // C             100
    // D             500
    // M             1000
    // 例如， 罗马数字 2 写做 II ，即为两个并列的 1。12 写做 XII ，即为 X + II 。 27 写做  XXVII, 即为 XX + V + II 。

    // 通常情况下，罗马数字中小的数字在大的数字的右边。但也存在特例，例如 4 不写做 IIII，而是 IV。数字 1 在数字 5 的左边，所表示的数等于大数 5 减小数 1 得到的数值 4 。同样地，数字 9 表示为 IX。这个特殊的规则只适用于以下六种情况：

    // I 可以放在 V (5) 和 X (10) 的左边，来表示 4 和 9。
    // X 可以放在 L (50) 和 C (100) 的左边，来表示 40 和 90。 
    // C 可以放在 D (500) 和 M (1000) 的左边，来表示 400 和 900。
    // 给你一个整数，将其转为罗马数字。
    // 输入: num = 58
    // 输出: "LVIII"
    // 解释: L = 50, V = 5, III = 3.
    readonly string[] thousands = {"", "M", "MM", "MMM"}; // 千位上的罗马数字表示
    readonly string[] hundreds  = {"", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"}; // 百位上的罗马数字表示
    readonly string[] tens      = {"", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"}; // 十位上的罗马数字表示
    readonly string[] ones      = {"", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"}; // 个位上的罗马数字表示

    public string IntToRoman(int num) {
        StringBuilder roman = new StringBuilder();
        roman.Append(thousands[num / 1000]); // 添加千位上的罗马数字
        roman.Append(hundreds[num % 1000 / 100]); // 添加百位上的罗马数字
        roman.Append(tens[num % 100 / 10]); // 添加十位上的罗马数字
        roman.Append(ones[num % 10]); // 添加个位上的罗马数字
        return roman.ToString(); // 返回最终的罗马数字表示
    }

    //13. 罗马数字转整数
    // 罗马数字包含以下七种字符: I， V， X， L，C，D 和 M。
    // 字符          数值
    // I             1
    // V             5
    // X             10
    // L             50
    // C             100
    // D             500
    // M             1000
    // 例如， 罗马数字 2 写做 II ，即为两个并列的 1 。12 写做 XII ，即为 X + II 。 27 写做  XXVII, 即为 XX + V + II 。
    // 通常情况下，罗马数字中小的数字在大的数字的右边。但也存在特例，例如 4 不写做 IIII，而是 IV。数字 1 在数字 5 的左边，所表示的数等于大数 5 减小数 1 得到的数值 4 。同样地，数字 9 表示为 IX。这个特殊的规则只适用于以下六种情况：
    // I 可以放在 V (5) 和 X (10) 的左边，来表示 4 和 9。
    // X 可以放在 L (50) 和 C (100) 的左边，来表示 40 和 90。 
    // C 可以放在 D (500) 和 M (1000) 的左边，来表示 400 和 900。
    // 给定一个罗马数字，将其转换成整数。
    //输入: s = "III"
    //输出: 3
    Dictionary<char, int> symbolValues = new Dictionary<char, int> {
        {'I', 1},
        {'V', 5},
        {'X', 10},
        {'L', 50},
        {'C', 100},
        {'D', 500},
        {'M', 1000},
    };

    public int RomanToInt(string s) {
        int ans = 0; // 用于存储最终的整数值
        int n = s.Length; // 获取输入字符串的长度

        for (int i = 0; i < n; ++i) {
            int value = symbolValues[s[i]]; // 获取当前字符对应的整数值

            // 如果当前字符的值小于下一个字符的值（前小后大的情况）
            if (i < n - 1 && value < symbolValues[s[i + 1]]) {
                ans -= value; // 减去当前字符的值
            } else {
                ans += value; // 否则，加上当前字符的值
            }
        }
        return ans; // 返回最终的整数表示
    }


    //14. 最长公共前缀
    //编写一个函数来查找字符串数组中的最长公共前缀。
    //如果不存在公共前缀，返回空字符串 ""。
    //输入：strs = ["flower","flow","flight"]
    //输出："fl"
    public string LongestCommonPrefix(string[] strs) {
        var result = new List<char>(); // 用于存储最长公共前缀的字符列表
        var length = strs.Min(s => s.Length); // 获取字符串数组中最短字符串的长度

        for (int i = 0; i < length; i++) {
            var set = new HashSet<char>(strs.Select(s => s[i])); // 创建字符集，包含当前位置的字符

            if (set.Count == 1) // 如果字符集中只包含一个字符，表示当前位置字符相同
                result.Add(strs[0][i]); // 添加到最长公共前缀结果中
            else
                break; // 否则，跳出循环，不再继续比较下去
        }

        return new string(result.ToArray()); // 将结果字符列表转换为字符串并返回
    }

    //15. 三数之和
    //给你一个整数数组 nums ，判断是否存在三元组 [nums[i], nums[j], nums[k]] 满足 i != j、i != k 且 j != k ，同时还满足 nums[i] + nums[j] + nums[k] == 0 。请
    //你返回所有和为 0 且不重复的三元组。
    //注意：答案中不可以包含重复的三元组。
    // 输入：nums = [-1,0,1,2,-1,-4]
    // 输出：[[-1,-1,2],[-1,0,1]]
    // 解释：
    // nums[0] + nums[1] + nums[2] = (-1) + 0 + 1 = 0 。
    // nums[1] + nums[2] + nums[4] = 0 + 1 + (-1) = 0 。
    // nums[0] + nums[3] + nums[4] = (-1) + 2 + (-1) = 0 。
    // 不同的三元组是 [-1,0,1] 和 [-1,-1,2] 。
    // 注意，输出的顺序和三元组的顺序并不重要。
        
    public IList<IList<int>> ThreeSum(int[] nums)
    {
        var result = new List<IList<int>>(); // 用于存储结果的列表

        // 对输入数组进行排序
        Array.Sort(nums);

        for (int i = 0; i < nums.Length - 2; i++)
        {
            int n1 = nums[i]; // 第一个数

            if (n1 > 0)
                break; // 如果第一个数大于0，后面的数都会大于0，不可能组成和为0的三元组，直接退出循环

            if (i > 0 && n1 == nums[i - 1])
                continue; // 避免重复处理相同的第一个数

            int left = i + 1; // 左指针
            int right = nums.Length - 1; // 右指针

            while (left < right)
            {
                int n2 = nums[left]; // 第二个数
                int n3 = nums[right]; // 第三个数
                int sum = n1 + n2 + n3; // 计算三个数的和

                if (sum > 0)
                {
                    right--; // 如果和大于0，将右指针向左移动
                }
                else if (sum < 0)
                {
                    left++; // 如果和小于0，将左指针向右移动
                }
                else
                {
                    result.Add(new List<int> { n1, n2, n3 }); // 如果和为0，将三个数添加到结果列表中

                    // 避免重复处理相同的第二个数和第三个数
                    while (left < right && nums[left] == n2)
                    {
                        left++;
                    }

                    while (left < right && nums[right] == n3)
                    {
                        right--;
                    }
                }
            }
        }

        return result; // 返回结果列表
    }






}
