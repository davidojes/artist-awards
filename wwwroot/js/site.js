let displayArtistsPromise = new Promise(function (resolve, reject) {
  fetch('/api/artist')
    .then(response => response.json())
    .then(artists => displayArtists(artists))
  //resolve(alert("Artists fetched successfully"));
});

displayArtistsPromise.then($(document).ready(function () {
  ($(".loader-wrapper").fadeOut("slow"));
}));


var artistsContainer = document.getElementById("artistsContainer");
var voteButtons;
var voteCheck = JSON.parse(localStorage.getItem("voteCheck"));

if (voteCheck == null) {
  voteCheck = { voted: false, artistId: -1 };
}


function displayArtists(artistArray) {
  console.log(artistArray);
  artistArray.forEach(function (artist) {
    var artistDiv = document.createElement("div");
    artistDiv.classList.add("artist-div");
    artistDiv.appendChild(createNameSpan(artist.name));
    var textDiv = document.createElement("span")
    var text = document.createElement("span")
    text.innerHTML = "Number of votes: "
    textDiv.appendChild(text)
    textDiv.appendChild(createVoteSpan(artist.votes, artist.id));
    artistDiv.appendChild(textDiv);
    artistDiv.appendChild(createVoteButton(artist.name, artist.id));
    artistsContainer.appendChild(artistDiv);
  })
  setupVoteButtons();
}


function setupVoteButtons() {
  voteButtons = Array.from(document.getElementsByClassName("vote-button"));

  voteButtons.forEach(function (voteButton) {
    var id = parseInt(voteButton.getAttribute("data-idButton"), 10);
    voteButton.addEventListener("click", function () { vote(id) });
  });
}


function vote(id) {
  var idJson = { Id: id };


  if (voteCheck.voted == true) {
    alert("Sorry, you have already voted");
  }
  else {
    fetch('/artist/api/vote/', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json;charset=utf-8'
      },
      body: JSON.stringify(idJson)
    })
      .then(updateVoteSpan(document.querySelector(`[data-idSpan="${id}"]`)))
      .then(updateVoteButton(document.querySelector(`[data-idButton="${id}"]`)))
      .then(storeVote(id));
  }
}


function updateVoteSpan(voteSpan) {
  let oldVote = voteSpan.innerHTML.trim();
  oldVote = parseInt(oldVote, 10);
  let newVote = oldVote + 1;
  voteSpan.innerHTML = newVote;
}

function updateVoteButton(voteButton) {
  voteButton.classList.add("voted-button");
}

function storeVote(artistId) {
  voteCheck.voted = true;
  voteCheck.artistId = artistId;
  localStorage.setItem("voteCheck", JSON.stringify(voteCheck));
}

function clearStorage() {
  localStorage.clear();
  voteCheck = { voted: false, artistId: -1 };
  let voteButton = document.querySelector("[class='vote-button voted-button']");
  voteButton.classList.remove("voted-button");
  alert("Local Storage Cleared");
}



// utility functions
function createNameSpan(artistName) {
  var nameSpan = document.createElement("span");
  nameSpan.classList.add("name-span");
  nameSpan.innerHTML = artistName;
  return nameSpan;
}

function createVoteSpan(votes, artistId) {
  var voteSpan = document.createElement("span");
  voteSpan.classList.add("vote-span");
  voteSpan.innerHTML = votes;
  voteSpan.setAttribute("data-idSpan", artistId);
  return voteSpan;
}


function createVoteButton(artistName, artistId) {
  var voteButton = document.createElement("button");
  voteButton.innerHTML = "Vote for " + artistName;
  voteButton.classList.add("vote-button");
  if (artistId == voteCheck.artistId) {
    voteButton.classList.add("voted-button");
  }
  voteButton.setAttribute("data-idButton", artistId);
  return voteButton;
}