using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private GameObject objectFound;
	public int score = 0;
	public AudioSource normalBugAudio;
	public AudioSource rareBugAudio;
	public AudioSource extremeRareBugAudio;
	public AudioSource roseliaAudio;
	public AudioSource beeDrillAudio;
	public AudioSource shotAudio;
	private GameObject animatedChild;
	private Animator anim;
	private int previousValue;
	private bool flag = false;
	public GameObject[] normalGameOjects;
	public GameObject[] rareGameOjects;
	[SerializeField] private TMP_Text scoreHolder;
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private GameObject hintPanel;
	[SerializeField] TMP_Text hintText;
	private ParticleSystem ps;

	public float timeValue;
	public TMP_Text timeText;
	[SerializeField] private GameObject time;
	public GameObject waitingScreen;
	public GameObject arCamera;
	private string currentHint;
	private string[,] normalhints =
		{
			{ "I am in charge of creating efficient and sustainable ways to grow crops and raise livestock. Who am I?", "I am responsible for designing and building structures that protect crops and livestock from the elements. Who am I?", "engineer ma mailo, Tractor kudaudai." },
			{ "I design and plan green spaces, parks, and recreational areas. Who am I?", "I use my imagination and drawing skills to create beautiful structures. Where am I made?", "sarkari jaagir gari khanxan bita bita, Kasle hola prayog garne hamro ramro itta." },
			{ "rat vari karayo dakshina harayo, yo thau vanda uta campus nai harayo.", "Engineering sakinxa namanauna haar, Yo batti bata khulyo bato char.", "vitra xiri bahira niskina ligau euta mode, bich ma basi jata hereni 90 degree code." },
			{ "uni gaye japan ma gaye qatar, euta connection bigriye chahinxa dherai taar.", "bijuli ko taar taar taar, fursad xaina ae hajur bhetau sukrabaar.", "In the land of circuits and wires, the solution to find the bug requires." },
			{ "Yeso bug maram vaneko, Bug ta binary/digital form ma po convert vaisakexa.", "code mai bug khoji hairan vako mah, delta ko bug ni ta oripari xa.", "nachna najanne aagan tedo, building chai mero chemical ko fohor chai tero?" },
			{ "Seek where the most of  first-year students go, the bug will be there and you'll know.", "Where most of 1st years education takes place, the treasure you seek is in this space.", "samanya bhasa ma bhannu parda, ma samanya xu" },
			{ "In the land of net and racquet, the answer to this riddle, you mustn't forget.", "Where gears and machines turn, Didn't you search here enough it's time to run?", "yeta heryo uta heryo yantra manab ra car, Bug hunt ni jitinxa kta ho namanau na haar." },
			{ "Where the food is served hot and fresh, the bug is attacking delicious dish.", "chiyapani khana gayeko kto panichiya mai chitta bujhayo.", "najik ko tirtha hela, hostel ka bidhyarthi aale tirai vela." },
			{ "Road ra BPR ko bich ma euta park, Kati dating baseko vai class tira vaag", " sunsari vitra jhapa morang vako eutai matra thau, Basi hera sochi hera k ho yesko naau?", "ek tira kitabai kitab arko tira Computer, Charai tira bug raixa now its your time to be shooter." },
			{ "I have machines and tools to turn raw materials into finished products. Find me", " I am responsible for cutting, shaping, and joining raw materials to create finished products. What am I?", "Bug hunt khelna ta vyaudaina sathi kheldeu mero sato,  khai kunni kata xa hera junkyard jane bato" },
		};
	private string[,] rareHints =
	   {
		  { "thulo kura haina karodau ko dwar(door), kasaile badhi bolyo vane assessment le prahar.", "Delta ma aaudaixa AC/DC band, yeha ko assignment ra assessment nagare final exam bata banned.", "Search where the voltage is high, the Bugs will be in plain sight." },
		  { "dajuvai ekarkako paripurak hami, Thulasana samrachana khadauna lai nami.", "haina mero teipani rakhxu euta gadi, chainxa gadi college ma jaba badxa jhadi.", "Drawing Gari, Daaldaal bata niskana hamlai kati garo, Bhok ra Nindra vanda hamlai assignments nai pyaro" },
		  { "To rule the campus, you must provide 'Masu-Bhat' for all, what am I? ", "sahitya padhe sir hunxa, kanxi lyaye ghar hunxa, College vitra xira matrai 'bunu, bunu' kai lahar hunxa", "lala guys hint-khoj-a-thon" },
		  { "na road xa na karod xa, bas bot, court ra ball khelne board xa", "roye runche, race track ma bato chekyo vani, ERC ko naam rakhyo jityo prize money", "buniya khana jau sathi haina saraswoti puja, yaha ko machine le katxa falam ambuja. " },
		  { "even if you don't manage to pass out within 8 years, you are trained to be a tractor driver.", " padhai? k ho tyo vanya, ma ta khet jotna aako.","manxe ta jotiyo jotiyo arko kura ni jotamla, college ma kei vayo ki tractor ma nikalamla."},
		  { "Aau sathi jado bhayo tapau esto kura, Ja basxan thulabada garxan sab lai hela", "lafda vayo hera sathi jau sabai mathi, kei kaam xaina yo thau ko tala hanau sathi", "kaam garne kalu makai khane valu, UGC ko paisa Bidhyarthi lai aalu." },
		  { "oh!  its useless channel gate, lets go friends its time to fabricate.", " bato wari bato pari ek ek ota shop, falam ghotney bahira aaune khaney aalu chop", "duita gate vanda xa euta arko gate, jata bata gayeni karyashala ma vet." },
		  { "kids do calculations in thousands/lakhs, mine starts from a crore.", "positive lai positive, negative lai negative, o sathi help gardeuna hunxa connective.", "You need me when you need money, but I am rarely available, who am I?" },
		  { "hatti xiryo pucchar adkiyo, 1st sem pass hau vaneko sadharan mai latkiyo.", "12 barsa ramayan padhayo sita kaski joi, 11 12 ma tetro padheko physics,  1st year ko class ma chai khai?", "samanya bhasa ma bhannu parda, ma samanya xu" },
		  { "department xa pari tya office ta yaha, kishan ra mistri aaye meeting basne kaha?", "jun aas lagdo thyo, lab ma uhi paryo vai, 1st year ma pass hunxu vaneko gultiyo ta dai", "jun aas lagdo, lab ma uhi paryo vai, 1st year ma pass hunxu vaneko gultiyo ta dai" },
		};
	public string[] finalHints = new string[20];

	static int Next(RNGCryptoServiceProvider random)
	{
		byte[] randomInt = new byte[4];
		random.GetBytes(randomInt);
		return Convert.ToInt32(randomInt[0]);
	}

	void Start()
	{
		StartCoroutine(HintTimer());
		RandomObject();
		RandomHint();
		if (HasScoreFile())
		{
			RetriveSaveScoreFile();
		}
		else
		{
			CreateSaveScoreFile();
		}
	}

	void RandomHint()
	{
		System.Random random = new System.Random();
		for (int i = 0; i < 20; i++)
		{
			if (i < 10)
			{
				finalHints[i] = normalhints[i, random.Next(0, 3)];
			}
			else
			{
				finalHints[i] = normalhints[i - 10, random.Next(0, 3)];
			}
		}
		RNGCryptoServiceProvider rnd = new();
		finalHints = finalHints.OrderBy(x => Next(rnd)).ToArray();
		currentHint = finalHints[0];
		hintText.text = currentHint;
	}
	void RandomObject()
	{
		System.Random random = new System.Random();
		for (int i = 0; i < normalGameOjects.Length; i++)
		{
			Transform transform = normalGameOjects[i].transform;
			int count = transform.childCount;
			GameObject go = transform.GetChild(random.Next(0, count)).gameObject;
			go.SetActive(true);
		}
		for (int i = 0; i < rareGameOjects.Length; i++)
		{
			Transform transform = rareGameOjects[i].transform;
			int count = transform.childCount;
			GameObject go = transform.GetChild(random.Next(0, count)).gameObject;
			go.SetActive(true);
		}
	}

	void Update()
	{
		if (timeValue > 0)
		{
			timeValue -= Time.deltaTime;

			if ((int)timeValue % 240 == 0 && flag == false)
			{
				previousValue = (int)timeValue;
				flag = true;
			}
			if (Convert.ToSingle(previousValue) > timeValue && flag == true && finalHints.Length > 1)
			{
				for (int i = 0; i < finalHints.Length - 1; i++)
				{
					finalHints[i] = finalHints[i + 1];
				}
				Array.Resize(ref finalHints, finalHints.Length - 1);
				currentHint = finalHints[0];
				hintText.text = currentHint;
				flag = false;
				previousValue = 0;
				StartCoroutine(HintTimer());
			}
		}
		else
		{
			SaveData();
		}
		if (HasScoreFile())
		{
			UpdateScoreFile();
		}
		else
		{
			CreateSaveScoreFile();
		}
		DisplayTime(timeValue);
		scoreHolder.text = "SCORE: " + score.ToString();
		scoreText.text = score.ToString();
	}

	IEnumerator HintTimer()
	{
		hintPanel.SetActive(true);
		yield return new WaitForSeconds(60);
		hintPanel.SetActive(false);
	}

	void SaveData()
	{
		waitingScreen.SetActive(true);
		arCamera.SetActive(false);
		StartCoroutine(ApplicationEnd());
	}

	void DisplayTime(float timeToDisplay)
	{
		if (timeToDisplay < 0)
		{
			timeToDisplay = 0;
		}
		else if (timeToDisplay > 0)
		{
			timeToDisplay += 1;

		}
		float hours = Mathf.FloorToInt((timeToDisplay / 3600) % 24);
		float minutes = Mathf.FloorToInt((timeToDisplay / 60) % 60);
		float seconds = Mathf.FloorToInt(timeToDisplay % 60);
		timeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
	}
	IEnumerator ApplicationEnd()
	{
		yield return new WaitForSeconds(6);
		EditScoreFile();
	}

	public void OnTargetFound(GameObject obj)
	{
		objectFound = obj;
		animatedChild = objectFound.transform.GetChild(0).gameObject;
		anim = animatedChild.GetComponent<Animator>();
		anim.SetBool("Idle", true);
		if (anim.runtimeAnimatorController.name != "FlyingBeedrill")
		{
			anim.SetBool("Dying", false);
		}

		if (objectFound.transform.parent.parent.name == "Rare")
		{
			rareBugAudio.Play();
		}
		else if (objectFound.transform.parent.parent.name == "Singular")
		{
			extremeRareBugAudio.Play();
		}
		else if (objectFound.transform.parent.name == "Roselia")
		{
			roseliaAudio.Play();
		}
		else
		{
			normalBugAudio.Play();
		}
	}

	public void OnTargetLost()
	{
		objectFound = null;
		anim = null;
	}

	public void ShootCurrentTarget(GameObject youKilledText)
	{

		shotAudio.Play();
		if (objectFound != null)
		{
			ps = objectFound.GetComponent<ParticleSystem>();
			if (ps != null)
			{
				ps.Play();
			}
			if (anim != null && anim.runtimeAnimatorController.name != "FlyingBeedrill")
			{
				anim.SetBool("Idle", false);
				anim.SetBool("Dying", true);
			}
			StartCoroutine(Dying(youKilledText));
		}
	}
	IEnumerator Dying(GameObject dyingText)
	{
		yield return new WaitForSeconds(1f);
		if (objectFound != null)
		{
			if (objectFound.transform.parent.parent.name == "Rare")
			{
				score += 4;
			}
			else if (objectFound.transform.parent.parent.name == "Singular")
			{
				score += 10;
			}
			else if (objectFound.transform.parent.parent.name == "GoodBugs")
			{
				score -= 3;
			}
			else
			{
				score += 3;
			}

			CreateScoreFile();
			StartCoroutine(Waiter(dyingText));
			objectFound.SetActive(false);
			Destroy(ps);
			objectFound = null;
		}

	}

	IEnumerator Waiter(GameObject youKilledText)
	{
		TMP_Text text = youKilledText.GetComponentInChildren<TMP_Text>();
		if(objectFound.transform.parent.parent.name == "GoodBugs")
		{
			text.text = "SORRY \n You killed the Good Bug.";
		}
		else
		{
			text.text = "CONGRATULATION \n You killed the Bad Bug.";
		}
		youKilledText.SetActive(true);
		yield return new WaitForSeconds(3);
		youKilledText.SetActive(false);
	}

	void CreateScoreFile()
	{
		string path = Application.persistentDataPath + "bugResult.txt";
		if (!(File.Exists(path)))
		{
			File.WriteAllText(path, "\n SCORES:\nTime Bug Found\t\tTime Left:\tScore\n");
		}
		string content = System.DateTime.Now + "\t" + timeText.text + "\t" + score + "\n";
		File.AppendAllText(path, content);
	}

	void EditScoreFile()
	{
		string path = Application.persistentDataPath + "bugResult.txt";

		string content = "\n----------------------\n" + "Total: " + scoreHolder.text + "\n----------------------\n";
		File.AppendAllText(path, content);
	}

	private void OnApplicationPause(bool pause)
	{
		string path = FilePath();
		if (pause == true)
		{

			string content = DateTime.Now + "\t" + timeText.text + "\t" + "Application Paused" + "\n";
			File.AppendAllText(path, content);
		}
		else
		{
			string content = DateTime.Now + "\t" + timeText.text + "\t" + "Application Started" + "\n";
			File.AppendAllText(path, content);

		}
	}
	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			string path = FilePath();
			string content = DateTime.Now + "\t" + timeText.text + "\t" + "Application Lost Focus" + "\n";
			File.AppendAllText(path, content);
		}
	}

	private string FilePath()
	{
		string path = Application.persistentDataPath + "bugResult.txt";
		if (!(File.Exists(path)))
		{
			File.WriteAllText(path, "SCORES:\nTime Bug Found\t\tTime Left:\tScore\n");
		}
		return path;
	}
	void CreateSaveScoreFile()
	{
		Debug.Log("File Created");
		string path = Application.persistentDataPath + "scores.json";
		MyData data = new();
		data.score = score;
		data.time = timeText.text;
		data.hints = finalHints;
		string json = JsonUtility.ToJson(data);
		File.WriteAllText(path, json);
	}
	void UpdateScoreFile()
	{
		string path = Application.persistentDataPath + "scores.json";
		MyData data = new();
		data.score = score;
		data.time = timeText.text;
		data.hints = finalHints;
		string json = JsonUtility.ToJson(data);
		File.WriteAllText(path, json);
	}
	void RetriveSaveScoreFile()
	{
		Debug.Log("File Retrive");
		string path = Application.persistentDataPath + "scores.json";
		string json = File.ReadAllText(path);
		MyData data = JsonUtility.FromJson<MyData>(json);
		score = data.score;
		timeText.text = data.time;
		finalHints = data.hints;

	}
	bool HasScoreFile()
	{
		string path = Application.persistentDataPath + "scores.json";
		if (!(File.Exists(path)))
		{
			return false;
		}
		return true;
	}

}
