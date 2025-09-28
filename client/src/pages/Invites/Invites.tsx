import Section from "@components/UI/Section";
import styles from "./Invites.module.scss";
import InvitesList from "@components/InvitesList";
import invitesQueries, { invitesCacheTags } from "@queries/api/invitesQueries";
import { useQuery } from "@tanstack/react-query";
import { eventParticipantViewModelsToCardProps } from "@utils/adapters/eventAdapter";

const InvitesPage = () => {
  const { data, isLoading, error } = useQuery({
    queryKey: [invitesCacheTags.index],
    queryFn: async () => {
      const res = await invitesQueries.getInvites();

      console.log(res.data);
      return res.data;
    },
    retry: false,
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error || !data) {
    return <div>Error loading data</div>;
  }

  return (
    <div className={styles.Invites}>
      <h1>Invites</h1>
      <div className={styles.InvitesSections}>
        <Section title="Incoming Invites" className={styles.InviteList}>
          <InvitesList
            invites={eventParticipantViewModelsToCardProps(
              data.incomingInvites
            )}
          />
        </Section>
        <Section title="Outgoing Invites" className={styles.InviteList}>
          <InvitesList
            invites={eventParticipantViewModelsToCardProps(
              data.outcomingInvites
            )}
          />
        </Section>
      </div>
    </div>
  );
};

export default InvitesPage;
