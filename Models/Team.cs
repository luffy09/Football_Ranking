using System.ComponentModel.DataAnnotations;

public class Team
{
	// ID is better as GUID but because this will be used in swagger(where we need to work with the ids) I am keeping it as string to be humanly readable
	[Key]
	public  string Id { get; set; }


	public  string Name { get; set; }

	public int TotalScore { get; set; } = 0;  // Win = 3, Draw = 1, Loss = 0

	public DateTime CreatedOn { get; set; } = DateTime.Now;
	public int isActive { get; set; } = 1;
	public int isDeleted { get; set; } = 0;

	public Team(string id, string name, int totalScore, DateTime createdOn, int isActive, int isDeleted)
	{
		Id = id;
		Name = name;
		TotalScore = totalScore;
		CreatedOn = createdOn;
		this.isActive = isActive;
		this.isDeleted = isDeleted;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Id, Name, TotalScore, CreatedOn, isActive, isDeleted);
	}
	public override bool Equals(object? obj)
	{
		if (obj is Team otherTeam)
		{
			// Check if all properties are the same
			return this.Id == otherTeam.Id &&
				   this.Name == otherTeam.Name &&
				   this.TotalScore == otherTeam.TotalScore &&
				   this.CreatedOn == otherTeam.CreatedOn &&
				   this.isActive == otherTeam.isActive &&
				   this.isDeleted == otherTeam.isDeleted;
		}
		return false;
	}
}