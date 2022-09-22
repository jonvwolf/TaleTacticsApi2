using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;

namespace HorrorTacticsApi2.Domain
{
    public class DefaultStoryCreatorService
    {
        // TODO: also delete the files
        //static readonly Tuple<string, int> ImageBodyChest = new("body_chest.jpg", 57344);
        //static readonly Tuple<string, int> ImageBodyEntranceGuard = new("body_entrance_guard.jpg", 57344);

        static readonly Tuple<string, int> ImageIntroTitle = new("intro_title.jpg", 47000);
        static readonly Tuple<string, int> ImageIntroGraveyard = new("intro_graveyard.jpg", 76000);
        static readonly Tuple<string, int> ImageIntroHouse = new("intro_house.jpg", 68000);
        static readonly Tuple<string, int> ImageIntroChestInsignia = new("body_chest_insignia.jpg", 60000);

        static readonly Tuple<string, int> ImageBodyCampfire = new("body_campfire.jpg", 55000);        
        static readonly Tuple<string, int> ImageBodyEntrance = new("body_entrance.jpg", 106000);
        static readonly Tuple<string, int> ImageBodyRamenRest = new("body_ramen_restaurant.jpg", 75000);
        static readonly Tuple<string, int> ImageBodyRamenRestKnight = new("body_ramen_restaurant_knight.jpg", 82000);

        static readonly Tuple<string, int> ImageEndJail = new("body_jail.jpg", 110000);
        static readonly Tuple<string, int> ImageEndToBeContinued = new("end_tobecontinued.jpg", 42000);
        static readonly Tuple<string, int> ImageEndInsignia = new("end_insignia.jpg", 39000);
        static readonly Tuple<string, int> ImageEndCredits = new("end_credits.jpg", 123000);
        static readonly Tuple<string, int> ImageEndCredits2 = new("end_tobecontinued_part2.jpg", 95000);

        static readonly Tuple<string, int, bool> SoundIntroTheme = new("intro_theme.mp3", 740000, false);
        static readonly Tuple<string, int, bool> SoundBodyCampfire = new("body_campfire.mp3", 522000, false);
        static readonly Tuple<string, int, bool> SoundBodyCastleTheme = new("body_castle_theme.mp3", 410000, false);
        static readonly Tuple<string, int, bool> SoundBodyMarketAmbience = new("body_market_ambience.mp3", 556000, false);
        static readonly Tuple<string, int, bool> SoundBodyKnight = new("body_knight.mp3", 174000, false);
        static readonly Tuple<string, int, bool> SoundEndJailTheme = new("end_fail.mp3", 556000, false);
        static readonly Tuple<string, int, bool> SoundEndCreditsTheme = new("credits_okay.mp3", 1250000, false);

        static readonly Tuple<string, int, bool> SoundEffectStomach = new("se_stomach.mp3", 3500, true);

        readonly StoryScenesService _storyScenesService; 
        readonly StoriesService _storiesService; 
        readonly StorySceneCommandsService _storySceneCommandsService;
        readonly IHorrorDbContext _context;
        public DefaultStoryCreatorService(StoryScenesService storyScenesService, StoriesService storiesService, StorySceneCommandsService storySceneCommandsService,
            IHorrorDbContext context)
        {
            _storiesService = storiesService;
            _storyScenesService = storyScenesService;
            _storySceneCommandsService = storySceneCommandsService;
            _context = context;
        }

        public async Task Create(UserEntity entity)
        {
            var imageIntroTitleEntity = CreateImage(ImageIntroTitle, entity);
            var imageIntroGraveyardEntity = CreateImage(ImageIntroGraveyard, entity);
            var imageIntroHouseEntity = CreateImage(ImageIntroHouse, entity);
            var imageIntroChestInsigniaEntity = CreateImage(ImageIntroChestInsignia, entity);
            var imageBodyCampfireEntity = CreateImage(ImageBodyCampfire, entity);
            var imageBodyEntranceEntity = CreateImage(ImageBodyEntrance, entity);
            var imageBodyRamenRestEntity = CreateImage(ImageBodyRamenRest, entity);
            var imageBodyRamenRestKnightEntity = CreateImage(ImageBodyRamenRestKnight, entity);
            var imageEndJailEntity = CreateImage(ImageEndJail, entity);
            var imageEndToBeContinuedEntity = CreateImage(ImageEndToBeContinued, entity);
            var imageEndInsigniaEntity = CreateImage(ImageEndInsignia, entity);
            var imageEndCreditsEntity = CreateImage(ImageEndCredits, entity);
            var imageEndCredits2Entity = CreateImage(ImageEndCredits2, entity);

            var soundIntroThemeEntity = CreateAudio(SoundIntroTheme, entity);
            var soundBodyCampfireEntity = CreateAudio(SoundBodyCampfire, entity);
            var soundBodyCastleThemeEntity = CreateAudio(SoundBodyCastleTheme, entity);
            var soundBodyMarketAmbienceEntity = CreateAudio(SoundBodyMarketAmbience, entity);
            var soundBodyKnightEntity = CreateAudio(SoundBodyKnight, entity);
            var soundEndJailThemeEntity = CreateAudio(SoundEndJailTheme, entity);
            var soundEndCreditsThemeEntity = CreateAudio(SoundEndCreditsTheme, entity);
            var soundEffectStomachEntity = CreateAudio(SoundEffectStomach, entity);
            

        }

        ImageEntity CreateImage(Tuple<string, int> info, UserEntity user)
        {
            var fileEntity = new FileEntity(info.Item1, FileFormatEnum.JPG, info.Item1, info.Item2, user)
            {
                IsDefault = true
            };

            var entity = new ImageEntity(fileEntity, 0, 0);
            _context.Images.Add(entity);
            return entity;
        }

        AudioEntity CreateAudio(Tuple<string, int, bool> info, UserEntity user)
        {
            var fileEntity = new FileEntity(info.Item1, FileFormatEnum.MP3, info.Item1, info.Item2, user)
            {
                IsDefault = true
            };

            var entity = new AudioEntity(fileEntity, info.Item3, 0);
            _context.Audios.Add(entity);
            return entity;
        }
    }
}
